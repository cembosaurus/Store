using Business.Exceptions.Interfaces;
using Business.Http.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;



namespace Business.Http
{
    public class HttpBaseService
    {

        private readonly IExId _exId;
        private static IHttpContextAccessor _accessor;
        private readonly bool _isProdEnv;
        private readonly IHttpAppClient _httpAppClient;
        private readonly IRemoteServicesInfoService _remoteServicesInfoService;

        protected IAppsettingsService _appsettingsService;
        protected ServiceURL_AS _serviceURL;
        protected HttpRequestMessage _requestMessage;
        protected string _remoteServiceName;
        protected string _remoteServicePathName;
        protected string _requestURL;
        protected string _requestQuery;
        protected HttpMethod _method;
        protected HttpContent _content;
        protected Dictionary<string, string> _headers = new Dictionary<string, string>();
        protected readonly string _mediaType = "application/json";
        protected readonly Encoding _encoding = Encoding.UTF8;
        protected static string _token => _accessor == null ? "" : _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "") ?? "";




        public HttpBaseService(IHostingEnvironment env, IHttpAppClient httpAppClient, IRemoteServicesInfoService remoteServicesInfoService)
        {
            _isProdEnv = env.IsProduction();
            _httpAppClient = httpAppClient;
            _remoteServicesInfoService = remoteServicesInfoService;
        }

        // to prevent circulatory DI with RemoteServicesInfoService. Other Http Services need RemoteServicesInfoService:
        public HttpBaseService(IHostingEnvironment env, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient)
        {
            _isProdEnv = env.IsProduction();
            _appsettingsService = appsettingsService;
            _httpAppClient = httpAppClient;
        }



        // VIRTUAL - unlike other HTTP Services, HttpManagementService doesn't need 503 ex checkup because ...
        // ... if Management API service is not reached then there is no logic in trying to update Management API service's URL from the same Management API service :)))
        protected async virtual Task<HttpResponseMessage> Send()
        {
            _requestURL = _serviceURL.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

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

                    var URLResult = await _remoteServicesInfoService.LoadURLs();

                    if (!URLResult.Status)
                        throw new HttpRequestException($"HTTP 503: Request to remote service '{_remoteServiceName}' could NOT be completed due to incorrect URL, and attempt to get correct URL from 'Management' Service FAILED ! \\n Message: {URLResult.Message}");


                    _requestURL = _serviceURL.GetUrlWithPath(TypeOfService.REST, _remoteServicePathName, _isProdEnv);

                    if (string.IsNullOrWhiteSpace(_requestURL))
                        throw new HttpRequestException("Could NOT get Remote Service URL ! Request URL was NOTY constructed !");

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


        protected void InitializeHttpRequestMessage()
        {
            _requestMessage = new HttpRequestMessage { RequestUri = new Uri(_requestURL + "/" + _requestQuery) };
            _requestMessage.Method = _method;
            _requestMessage.Content = _content;
            _requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
            if(!string.IsNullOrWhiteSpace(_token))
                _requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            if (!_headers.IsNullOrEmpty())
            {
                foreach (var h in _headers)
                {
                    _requestMessage.Headers.Add(h.Key, h.Value);
                }
            }
            _headers.Clear();
        }



        protected bool AddApiKeyToHeader()
        {
            var apiKeyResult = _appsettingsService.GetApiKey();

            if (apiKeyResult.Status)
            {
                _headers.Add("ApiKey", apiKeyResult.Data);

                return true;
            }

            return false;
        }


        protected void Initialize()
        {
            _serviceURL = _appsettingsService.GetRemoteServiceURL(_remoteServiceName).Data;
        }

    }
}
