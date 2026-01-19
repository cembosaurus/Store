using Business.Exceptions.Interfaces;
using Business.Http.Clients.Interfaces;
using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;



namespace Business.Http.Services
{
    public class HttpBaseService : IHttpBaseService
    {
        private static IHttpContextAccessor _accessor;
        private readonly IHttpAppClient _httpAppClient;
        private readonly bool _isProdEnv;
        private readonly string _appName;

        protected readonly IServiceResultFactory _resultFact;
        private readonly ConsoleWriter _cw;

        protected IGlobalConfig_PROVIDER _globalConfig_Provider;
        protected readonly IExId _exId;

        protected bool _useApiKey;
        protected RemoteService_AS_MODEL _service_model;
        protected HttpRequestMessage _requestMessage;
        protected string _remoteServiceName = "";
        protected string _remoteServicePathName = "";
        protected string _requestURL = "";
        protected string _requestQuery = "";
        protected HttpMethod _method;
        protected HttpContent _content;
        protected Dictionary<string, string> _requestHeaders = new Dictionary<string, string>();
        protected readonly string _mediaType = "application/json";
        protected readonly Encoding _encoding = Encoding.UTF8;
        protected static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";



        // any Http service (except 'HTTPManagementService'):
        public HttpBaseService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER globalConfig_Provider, IServiceResultFactory resultFact, ConsoleWriter cw)
        {
            _accessor = accessor;
            _appName = env.ApplicationName;
            _isProdEnv = env.IsProduction();
            _exId = exId;
            _httpAppClient = httpAppClient;
            _globalConfig_Provider = globalConfig_Provider;
            _resultFact = resultFact;
            _cw = cw;
        }
        // 'HTTPManagementService': to prevent circulatory DI: HTTPManagementService <--> GlobalConfig_PROVIDER:
        public HttpBaseService(IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, ConsoleWriter cm)
        {
            _appName = env.ApplicationName;
            _isProdEnv = env.IsProduction();
            _exId = exId;
            _httpAppClient = httpAppClient;
            _resultFact = resultFact;
            _cw = cm;
        }





        protected async Task<IServiceResult<T>> HTTP_Request_Handler<T>()
        {
            // create HTTP request:

            var createReqResponse = await Prepare_HTTPRequest();

            if (!createReqResponse.Status)
                return _resultFact.Result(default(T), false, $"HTTP request from '{_appName}' to '{_remoteServiceName}' was NOT created ! Reason: {createReqResponse.Message}");


            // send HTTP reequest:

            var responseMessage = await Send();


            // response handling:

            var content = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode || responseMessage.Content.GetType().Name == "EmptyContent")
            {

                // HTTP request to update local Global Config Failed:

                var response = _resultFact.Result(default(T), false, $"{responseMessage.ReasonPhrase}: {responseMessage.RequestMessage?.Method}/{responseMessage.RequestMessage?.RequestUri}.   Error: '{content}'");

                return response;
            }
                        
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<T>>(content) ?? _resultFact.Result<T>(default, responseMessage.IsSuccessStatusCode, responseMessage.ReasonPhrase ?? "");
            
            return result;

        }




        // VIRTUAL - unlike all HTTP Services, HttpManagementService (uses its own overridden Send()) doesn't need 503 ex handling because
        // if Management API service is not reached via HTTP request, then there is no logic in trying to update Management API service's URL by response from Management API service:
        protected async virtual Task<HttpResponseMessage> Send()
        {
            // FIRST attempt:

            try
            {
                return await _httpAppClient.SendAsync(_requestMessage);
            }
            catch (Exception ex) when (_exId.IsHttp_503(ex))
            {

                // Catch ex 503: if HTTP Request URL provided by global settings is not up-to-date or wrong, request will fail ! ...
                // Send the HTTP request to Management API service to update global settings by new data. ...
                // Create new request URL. ...
                // Try HTTP request again:


                var GCResult = await DownloadGloalConfig();

                if (!GCResult.Status)
                    return _requestMessage.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, $"HTTP 503: Request to API service '{_remoteServiceName}' could NOT be completed due to incorrect URL. Attempt to download Global Config with correct URL from 'Management' API service FAILED ! \\n Message: {GCResult.Message}", ex);


                _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

                if (string.IsNullOrWhiteSpace(_requestURL))
                    return _requestMessage.CreateErrorResponse(HttpStatusCode.InternalServerError, $"URL for '{_remoteServiceName}' was not fdound in local Global Config !", ex);
            }


            try
            {
                // SECOND attempt:

                Create_HttpRequestMessage();

                return await _httpAppClient.SendAsync(_requestMessage);
            }
            catch (Exception ex) when (_exId.IsHttp_503(ex))
            {
                //throw new HttpResponseException(_requestMessage.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, $"HTTP 503: Response from '{_remoteServiceName}' not received! {ex.Message}", ex));

                // add service name into error message:
                return _requestMessage.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, $"HTTP 503: Response from '{_remoteServiceName}' not received! {ex.Message}", ex);
            }

        }



        protected async virtual Task<IServiceResult<bool>> Prepare_HTTPRequest()
        {

            if (string.IsNullOrWhiteSpace(_remoteServiceName))
                return _resultFact.Result(false, false, $"Remote Service NOT found, service's name was NOT provided !");

            // Load service spec from local Global Config
            var modelResult = GetServiceModel_FromLocalGlobalConfig();

            if (!modelResult.Status)
            {
                // Local Global Config could be obsolete or incomplete.
                // Send HHTP Get requiest to Management API service to update local Global Config,
                // and try toi build HTTP request again.


                // Update local Global Config data from remote API source (Management Service):
                var GCResult = await DownloadGloalConfig();

                if (!GCResult.Status)
                    return _resultFact.Result(false, false, $"URL for service '{_remoteServiceName}' is not available, because attempt to download Global Config with correct URL from Management API service FAILED! Message: {GCResult.Message}");

                // Load service model from local Global Config again:
                modelResult = GetServiceModel_FromLocalGlobalConfig();

                if (!modelResult.Status)
                    return _resultFact.Result(false, false, $"Config data with URL for service '{_remoteServiceName}' was NOT found in local Global Config !");
            }

            _service_model = modelResult.Data ?? null!;


            // URL found in local Global Config,
            // build HTTP request URL:

            _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

            if (string.IsNullOrWhiteSpace(_requestURL))
                return _resultFact.Result(false, false, $"Request URL string for service '{_remoteServiceName}' could not be constructed ! Missing data in local Global Config !");


            // use API Key for relevant API services:

            _useApiKey = _remoteServicePathName == "GlobalConfig" || _remoteServicePathName == "Collector";

            if (_useApiKey)
            {
                var apiKeyResult = AddApiKeyToHeader();

                if (!apiKeyResult.Status)
                    return apiKeyResult;
            }


            // create HTTP request message, insert headers:

            Create_HttpRequestMessage();


            return _resultFact.Result(true, true);
        }



        protected void Create_HttpRequestMessage()
        {
            _requestMessage = new HttpRequestMessage { RequestUri = new Uri(_requestURL + (string.IsNullOrWhiteSpace(_requestQuery) ? "" : "/" + _requestQuery)) };
            _requestMessage.Method = _method;
            _requestMessage.Content = _content;

            // Headers:
            _requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

            if (!string.IsNullOrWhiteSpace(_token) && !_useApiKey)
                _requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            if (_requestHeaders.Any())
            {
                foreach (var h in _requestHeaders)
                {
                    _requestMessage.Headers.Add(h.Key, h.Value);
                }
            }
            _requestHeaders.Clear();
        }



        protected virtual IServiceResult<bool> AddApiKeyToHeader()
        {
            var apiKeyResult = _globalConfig_Provider.GetApiKey();

            if (apiKeyResult.Status)
                _requestHeaders.Add("x-api-key", apiKeyResult.Data ?? "");

            return _resultFact.Result(apiKeyResult.Status, apiKeyResult.Status, apiKeyResult.Message);
        }



        protected virtual IServiceResult<RemoteService_AS_MODEL> GetServiceModel_FromLocalGlobalConfig()
        {
            return _globalConfig_Provider.GetRemoteServiceByName(_remoteServiceName);
        }


        protected virtual async Task<IServiceResult<Config_Global_AS_MODEL>> DownloadGloalConfig()
        {
            var GCResult = await _globalConfig_Provider.DownloadGlobalConfig();

            if (!GCResult.Status)
                return _resultFact.Result<Config_Global_AS_MODEL>(null, false, GCResult.Message);

            return GCResult;
        }




        public string GetRequestURL { get { return _requestMessage.RequestUri?.ToString() ; } }

        public string GetRemoteServiceName { get { return _remoteServiceName; } }
    }
}





//using Business.Exceptions.Interfaces;
//using Business.Http.Clients.Interfaces;
//using Business.Http.Services.Interfaces;
//using Business.Libraries.ServiceResult;
//using Business.Libraries.ServiceResult.Interfaces;
//using Business.Management.Appsettings.Models;
//using Business.Management.Enums;
//using Business.Management.Services.Interfaces;
//using Business.Tools;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Hosting;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.Net.Http.Headers;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Text;



//namespace Business.Http.Services
//{
//    public class HttpBaseService : IHttpBaseService
//    {

//        private static IHttpContextAccessor _accessor;
//        private readonly IHttpAppClient _httpAppClient;
//        private readonly bool _isProdEnv;

//        protected readonly IServiceResultFactory _resultFact;
//        private readonly ConsoleWriter _cw;
//        protected IGlobalConfig_PROVIDER _globalConfig_Provider;
//        protected readonly IExId _exId;

//        protected bool _useApiKey;
//        protected RemoteService_AS_MODEL _service_model;
//        protected HttpRequestMessage _requestMessage;
//        protected string _remoteServiceName = "";
//        protected string _remoteServicePathName = "";
//        protected string _requestURL = "";
//        protected string _requestQuery = "";
//        protected HttpMethod _method;
//        protected HttpContent _content;
//        protected Dictionary<string, string> _requestHeaders = new Dictionary<string, string>();
//        protected readonly string _mediaType = "application/json";
//        protected readonly Encoding _encoding = Encoding.UTF8;
//        protected static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";



//        // any Http service (except 'HTTPManagementService'):
//        public HttpBaseService(IHttpContextAccessor accessor, IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IGlobalConfig_PROVIDER globalConfig_Provider, IServiceResultFactory resultFact, ConsoleWriter cw)
//        {
//            _accessor = accessor;
//            _isProdEnv = env.IsProduction();
//            _exId = exId;
//            _httpAppClient = httpAppClient;
//            _globalConfig_Provider = globalConfig_Provider;
//            _resultFact = resultFact;
//            _cw = cw;
//        }
//        // 'HTTPManagementService': to prevent circulatory DI: HTTPManagementService <--> GlobalConfig_PROVIDER:
//        public HttpBaseService(IWebHostEnvironment env, IExId exId, IHttpAppClient httpAppClient, IServiceResultFactory resultFact, ConsoleWriter cm)
//        {
//            _isProdEnv = env.IsProduction();
//            _exId = exId;
//            _httpAppClient = httpAppClient;
//            _resultFact = resultFact;
//            _cw = cm;
//        }





//        protected async Task<IServiceResult<T>> HTTP_Request_Handler<T>()
//        {
//            // create HTTP request:

//            var createReqResponse = await Prepare_HTTPRequest();

//            if (!createReqResponse.Status)
//                return _resultFact.Result(default(T), false, $"HTTP request to '{_remoteServiceName}' was NOT created ! Reason: {createReqResponse.Message}");


//            // send HTTP reequest:

//            var responseMessage = await Send();
            
            
//            if (!responseMessage.IsSuccessStatusCode || responseMessage.Content.GetType().Name == "EmptyContent")
//            {
            
//                // HTTP request to update local Global Config Failed:
            
//                var response = _resultFact.Result(default(T), false, $"{responseMessage.ReasonPhrase}: {responseMessage.RequestMessage?.Method}, {responseMessage.RequestMessage?.RequestUri}");
            
//                return response;
//            }
            
//            var content = responseMessage.Content.ReadAsStringAsync().Result;
            
//            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<T>>(content) ?? _resultFact.Result<T>(default, responseMessage.IsSuccessStatusCode, responseMessage.ReasonPhrase ?? "");
            
//            return result;

//        }




//        // VIRTUAL - unlike all HTTP Services, HttpManagementService (uses its own overridden Send()) doesn't need 503 ex handling because
//        // if Management API service is not reached via HTTP request, then there is no logic in trying to update Management API service's URL from Management API service:
//        protected async virtual Task<HttpResponseMessage> Send()
//        {

//            // FIRST attempt:

//            try
//            {
//                return await _httpAppClient.SendAsync(_requestMessage);
//            }
//            catch (Exception ex) when (_exId.Http_503(ex))
//            {

//                // Catch ex 503: if HTTP Request URL provided by global settings is not up-to-date or wrong, request will fail ! ...
//                // Send the HTTP request to Management API service to update global settings by new data. ...
//                // Create new request URL. ...
//                // Try HTTP request again:


//                var GCResult = await DownloadGloalConfig();

//                if (!GCResult.Status)
//                    return _requestMessage.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, $"HTTP 503: Request to API service '{_remoteServiceName}' could NOT be completed due to incorrect URL. Attempt to download Global Config with correct URL from 'Management' API service FAILED ! \\n Message: {GCResult.Message}", ex);


//                _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

//                if (string.IsNullOrWhiteSpace(_requestURL))
//                    return _requestMessage.CreateErrorResponse(HttpStatusCode.InternalServerError, $"URL for '{_remoteServiceName}' was not fdound in local Global Config !", ex);
//            }


//            try
//            {
//                // SECOND attempt:

//                Create_HttpRequestMessage();

//                return await _httpAppClient.SendAsync(_requestMessage);
//            }
//            catch (Exception ex) when (_exId.Http_503(ex))
//            {                
//                return _requestMessage.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, $"HTTP 503: Respoonse from '{_remoteServiceName}'. Message: {ex.Message}", ex);
//            }

//        }



//        protected async virtual Task<IServiceResult<bool>> Prepare_HTTPRequest()
//        {

//            if (string.IsNullOrWhiteSpace(_remoteServiceName))
//                return _resultFact.Result(false, false, $"Remote Service NOT found, service's name was NOT provided !");

//            // Load service spec from local Global Config
//            var modelResult = GetServiceModel_FromLocalGlobalConfig();

//            if (!modelResult.Status)
//            {
//                // Local Global Config could be obsolete or incomplete.
//                // Send HHTP Get requiest to Management API service to update local Global Config,
//                // and try toi build HTTP request again.


//                // Update local Global Config data from remote API source (Management Service):
//                var GCResult = await DownloadGloalConfig();

//                if (!GCResult.Status)
//                    return _resultFact.Result(false, false, $"URL for service '{_remoteServiceName}' is not available, because attempt to download Global Config with correct URL from Management API service FAILED! Message: {GCResult.Message}");

//                // Load service model from local Global Config again:
//                modelResult = GetServiceModel_FromLocalGlobalConfig();

//                if (!modelResult.Status)
//                    return _resultFact.Result(false, false, $"Config data with URL for service '{_remoteServiceName}' was NOT found in local Global Config !");
//            }

//            _service_model = modelResult.Data ?? null!;


//            // URL found in local Global Config,
//            // build HTTP request URL:

//            _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

//            if (string.IsNullOrWhiteSpace(_requestURL))
//                return _resultFact.Result(false, false, $"Request URL string for service '{_remoteServiceName}' could not be constructed ! Missing data in local Global Config !");


//            // use API Key for relevant API services:

//            _useApiKey = _remoteServicePathName == "GlobalConfig" || _remoteServicePathName == "Collector";

//            if (_useApiKey)
//            {
//                var apiKeyResult = AddApiKeyToHeader();

//                if (!apiKeyResult.Status)
//                    return apiKeyResult;
//            }


//            // create HTTP request message, insert headers:

//            Create_HttpRequestMessage();


//            return _resultFact.Result(true, true);
//        }



//        protected void Create_HttpRequestMessage()
//        {
//            _requestMessage = new HttpRequestMessage { RequestUri = new Uri(_requestURL + (string.IsNullOrWhiteSpace(_requestQuery) ? "" : "/" + _requestQuery)) };
//            _requestMessage.Method = _method;
//            _requestMessage.Content = _content;
//            _requestMessage.Options.Set(new HttpRequestOptionsKey<string>("RequestTo"), _remoteServiceName);

//            // Headers:
//            _requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));

//            if (!string.IsNullOrWhiteSpace(_token) && !_useApiKey)
//                _requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

//            if (!_requestHeaders.IsNullOrEmpty())
//            {
//                foreach (var h in _requestHeaders)
//                {
//                    _requestMessage.Headers.Add(h.Key, h.Value);
//                }
//            }
//            _requestHeaders.Clear();
//        }



//        protected virtual IServiceResult<bool> AddApiKeyToHeader()
//        {
//            var apiKeyResult = _globalConfig_Provider.GetApiKey();

//            if (apiKeyResult.Status)
//                _requestHeaders.Add("x-api-key", apiKeyResult.Data ?? "");

//            return _resultFact.Result(apiKeyResult.Status, apiKeyResult.Status, apiKeyResult.Message);
//        }



//        protected virtual IServiceResult<RemoteService_AS_MODEL> GetServiceModel_FromLocalGlobalConfig()
//        {
//            return _globalConfig_Provider.GetRemoteServiceByName(_remoteServiceName);
//        }


//        protected virtual async Task<IServiceResult<Config_Global_AS_MODEL>> DownloadGloalConfig()
//        {
//            var GCResult = await _globalConfig_Provider.DownloadGlobalConfig();

//            if (!GCResult.Status)
//                return _resultFact.Result<Config_Global_AS_MODEL>(null, false, GCResult.Message);

//            return GCResult;
//        }




//        public string GetRequestURL { get { return _requestMessage.RequestUri?.ToString() ; } }

//        public string GetRemoteServiceName { get { return _remoteServiceName; } }
//    }
//}
