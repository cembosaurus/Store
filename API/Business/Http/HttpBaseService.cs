using Business.Exceptions.Interfaces;
using Business.Http.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings;
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
        private readonly IRemoteServicesInfo_Provider _remoteServicesInfo_Provider;
        private IAppsettingsService _appsettingsService;
        private readonly IExId _exId;
        private readonly bool _isProdEnv;
        private readonly IServiceResultFactory _resultFact;

        protected Service_Model_AS _service_model;
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




        public HttpBaseService(IHostingEnvironment env, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IRemoteServicesInfo_Provider remoteServicesInfo_Provider, IServiceResultFactory resultFact)
        {
            _isProdEnv = env.IsProduction();
            _appsettingsService = appsettingsService;
            _httpAppClient = httpAppClient;
            _remoteServicesInfo_Provider = remoteServicesInfo_Provider;
            _resultFact = resultFact;
        }

        // to prevent circulatory DI with RemoteServicesInfoService. Other Http Services need RemoteServicesInfoService:
        public HttpBaseService(IHostingEnvironment env, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
        {
            _isProdEnv = env.IsProduction();
            _appsettingsService = appsettingsService;
            _httpAppClient = httpAppClient;
            _resultFact = resultFact;
        }



        // VIRTUAL - unlike other HTTP Services, HttpManagementService doesn't need 503 ex checkup because ...
        // ... if Management API service is not reached then there is no logic in trying to update Management API service's URL from the same Management API service :)))
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

                    var servicesModelsResult = await _remoteServicesInfo_Provider.LoadServiceModels();

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


        //------------------------------------------------------------------------------------------------------------------------------------------------------- To DO: if not Management then model is NULL -> let it be handled in SEND() !!!!!! reloading from Management
        protected IServiceResult<bool> InitializeRequest()
        {
            var serviceResult = _appsettingsService.GetRemoteServiceURL(_remoteServiceName);
            if (!serviceResult.Status)
                return _resultFact.Result(false, false, $"Remote Service Info model '{_remoteServiceName}' was NOT found in Appsettings !");

            _service_model = serviceResult.Data;

            _requestURL = _service_model.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);
            if (string.IsNullOrWhiteSpace(_requestURL))
                return _resultFact.Result(false, false, $"Request URL for Remote Service '{_remoteServiceName}' could not be constructed ! Missing data in Appsettings !");

            var apiKeyResult = _appsettingsService.GetApiKey();
            if (!apiKeyResult.Status)
                return _resultFact.Result(apiKeyResult.Status, false, $"{apiKeyResult.Message}");

            _requestHeaders.Add("ApiKey", apiKeyResult.Data);

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

    }
}
