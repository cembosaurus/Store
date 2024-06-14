using Business.Exceptions.Interfaces;
using Business.Http.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http.Headers;
using System.Text;



namespace Business.Http
{
    public class HttpBaseService
    {

        private static IHttpContextAccessor _accessor;
        private readonly IHttpAppClient _httpAppClient;
        private readonly IRemoteServices_PROVIDER _remoteServices_Provider;
        private IAppsettings_PROVIDER _appsettings_Provider;
        private readonly IExId _exId;
        private readonly bool _isProdEnv;

        protected readonly IServiceResultFactory _resultFact;

        protected bool _useApiKey;
        protected RemoteService_MODEL_AS _service_model;
        protected HttpRequestMessage _requestMessage;
        protected string _remoteServiceName;
        protected string _remoteServicePathName;
        protected string _requestURL;
        protected string _requestQuery;
        protected HttpMethod _method;
        protected HttpContent _content;
        protected Dictionary<string, string> _requestHeaders = new Dictionary<string, string>();
        protected readonly string _mediaType = "application/json";
        protected readonly Encoding _encoding = Encoding.UTF8;
        protected static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";




        public HttpBaseService(IHostingEnvironment env, IExId exId, IAppsettings_PROVIDER appsettings_Provider, IHttpAppClient httpAppClient, IRemoteServices_PROVIDER remoteServices_Provider, IServiceResultFactory resultFact)
        {
            _isProdEnv = env.IsProduction();
            _exId = exId;
            _appsettings_Provider = appsettings_Provider;
            _httpAppClient = httpAppClient;
            _remoteServices_Provider = remoteServices_Provider;
            _resultFact = resultFact;
        }
        // For 'HTTPManagementService', to maintain business logic and prevent circulatory DI: HTTPManagementService <--> RemoteServicesInfoService:
        public HttpBaseService(IHostingEnvironment env, IAppsettings_PROVIDER appsettings_Provider, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
        {
            _isProdEnv = env.IsProduction();
            _appsettings_Provider = appsettings_Provider;
            _httpAppClient = httpAppClient;
            _resultFact = resultFact;
        }




        protected async Task<IServiceResult<T>> HTTP_Request_Handler<T>()
        {
            var initResponse = await InitializeRequest();

            if (!initResponse.Status)
                return _resultFact.Result(default(T), false, $"Request for remote service '{_remoteServiceName}' was NOT initialized ! Reason: {initResponse.Message}");

            var sendResponse = await Send();

            _useApiKey = false;

            if (!sendResponse.IsSuccessStatusCode || sendResponse.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result(default(T), false, $"{(sendResponse.ReasonPhrase == "OK" ? "Fail" : sendResponse.ReasonPhrase)}: {sendResponse.RequestMessage?.Method}, {sendResponse.RequestMessage?.RequestUri}");

            var content = sendResponse.Content.ReadAsStringAsync().Result ?? "";

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<T>>(content);

            return result;
        }



        // VIRTUAL - unlike all HTTP Services, HttpManagementService (uses its own overridden Send()) doesn't need 503 ex checkup because
        // if Management API service is not reached then there is no logic in trying to update Management API service's URL from this Management API service
        protected async virtual Task<HttpResponseMessage> Send()
        {
            _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

            if (!string.IsNullOrWhiteSpace(_requestURL))
            {
                try
                {
                    InitializeHttpRequestMessage();

                    return await _httpAppClient.Send(_requestMessage);
                }
                catch (Exception ex) when (_exId.Http_503(ex))
                {
                    // Catch ex 503: local URL definition is obsolete or wrong, request failed !
                    // Update Remote Service URL to avoid 503 exception and try request again:

                    var servicesModelsResult = await ReLoadServices();

                    if (!servicesModelsResult.Status)
                        throw new HttpRequestException($"HTTP 503: Request to remote service '{_remoteServiceName}' could NOT be completed due to incorrect URL, and attempt to get correct URL from 'Management' Service FAILED ! \\n Message: {servicesModelsResult.Message}");


                    _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

                    if (string.IsNullOrWhiteSpace(_requestURL))
                        throw new HttpRequestException("Could NOT get Remote Service URL ! Request URL was NOT constructed !");

                    try
                    {
                        // Rebuild the http request message, to prevent "Request already sent" error:
                        InitializeHttpRequestMessage();

                        return await _httpAppClient.Send(_requestMessage);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return _requestMessage.CreateErrorResponse(HttpStatusCode.BadRequest, "Request URL was NOT constructed !");
        }


        protected async Task<IServiceResult<bool>> InitializeRequest()
        {
            //___ THIS Service Model:

            if(string.IsNullOrWhiteSpace(_remoteServiceName))
                return _resultFact.Result(false, false, $"Remote Service name was NOT provided !");

            var modelResult = GetServiceModel();

            if (!modelResult.Status)
            {
                var result = await ReLoadServices();

                if(!result.Status)
                    return _resultFact.Result(false, false, $"Failed to fetch Remote Services Info models from Management service !");

                modelResult = _remoteServices_Provider.GetServiceByName(_remoteServiceName);
                if (!modelResult.Status)
                    return _resultFact.Result(false, false, $"Remote Service Info model '{_remoteServiceName}' was NOT found !");
            }

            _service_model = modelResult.Data;


            //___ Request URL:

            _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

            if (string.IsNullOrWhiteSpace(_requestURL))
                return _resultFact.Result(false, false, $"Request URL for Remote Service '{_remoteServiceName}' could not be constructed ! Missing data in Appsettings !");


            //___ API Key:

            if(_useApiKey)
            { 
                var apiKeyResult = AddApiKeyToHeader();

                if (!apiKeyResult.Status)
                    return apiKeyResult;
            }


            //___ HTTP Message:

            InitializeHttpRequestMessage();


            return _resultFact.Result(true, true);
        }


        protected void InitializeHttpRequestMessage()
        {
            _requestMessage = new HttpRequestMessage { RequestUri = new Uri(_requestURL + (string.IsNullOrWhiteSpace(_requestQuery) ? "" : "/" + _requestQuery)) };
            _requestMessage.Method = _method;
            _requestMessage.Content = _content;
            _requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            if (!string.IsNullOrWhiteSpace(_token))
                _requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            if (!_requestHeaders.IsNullOrEmpty())
            {
                foreach (var h in _requestHeaders)
                {
                    _requestMessage.Headers.Add(h.Key, h.Value);
                }
            }
            _requestHeaders.Clear();
        }


        protected IServiceResult<bool> AddApiKeyToHeader()
        {
            var apiKeyResult = _appsettings_Provider.GetApiKey();

            if(apiKeyResult.Status)
                _requestHeaders.Add("x-api-key", apiKeyResult.Data ?? "");

            return _resultFact.Result(apiKeyResult.Status, apiKeyResult.Status, $"HTTP Request to {_remoteServiceName}/{_remoteServicePathName}: {apiKeyResult.Message}");
        }


        private async Task<IServiceResult<IEnumerable<RemoteService_MODEL_AS>>> ReLoadServices()
        { 
            return await _remoteServices_Provider.ReLoad();
        }


        protected virtual IServiceResult<RemoteService_MODEL_AS> GetServiceModel()
        {
            return _remoteServices_Provider.GetServiceByName(_remoteServiceName);
        }

    }
}
