using Business.Exceptions.Interfaces;
using Business.Http.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;



namespace Business.Http
{
    public class HttpBaseService
    {

        private readonly IExId _exId;
        private static IHttpContextAccessor _accessor;
        private readonly IHttpAppClient _httpAppClient;
        private readonly IRemoteServicesInfoService _remoteServicesInfoService;

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




        public HttpBaseService(IHttpAppClient httpAppClient, IRemoteServicesInfoService remoteServicesInfoService)
        {
            _httpAppClient = httpAppClient;
            _remoteServicesInfoService = remoteServicesInfoService;
        }

        // to prevent circulatory DI with RemoteServicesInfoService. Other Http Services need RemoteServicesInfoService:
        public HttpBaseService(IHttpAppClient httpAppClient)
        {
            _httpAppClient = httpAppClient;
        }



        // VIRTUAL - unlike other HTTP Services, HttpManagementService doesn't need 503 ex checkup because ...
        // ... if Management API service is not reached then there is no logic in trying to update Management API service's URL from the same Management API service :)))
        protected async virtual Task<HttpResponseMessage> Send()
        {
            var URLStringResult = BuildServiceURL();

            if (URLStringResult.Status)
                _requestURL = BuildServiceURL().Data ?? "";







            //.............................................................. URL DB should be loaded before this request on app startup. !!!!!!!!! But if Management service is not ON on startup then load URLs now !!!

            // ................................... Handle the URLs reload on empty DB in RemoteServicesInfoService ????????????????????







            try
            {
                InitializeHttpRequestMessage();

                return await _httpAppClient.Send(_requestMessage);
            }
            catch (Exception ex) when (_exId.Http_503(ex))
            {
                // Catch ex 503: local URL definition is obsolete or wrong, request failed !
                // Update Remote Service URL to avoid 503 exception and try request again:

                var URLResult = await _remoteServicesInfoService.LoadAllRemoteServicesURL();

                if (!URLResult.Status)
                    throw new HttpRequestException($"HTTP 503: Request to remote service '{_remoteServiceName}' could NOT be completed due to incorrect URL, and attempt to get correct URL from 'Management' Service FAILED ! \\n Message: {URLResult.Message}");


                URLStringResult = BuildServiceURL();

                if (!URLStringResult.Status)
                    throw new HttpRequestException($"Could NOT get Remote Service URL ! Reason: '{URLStringResult.Message}'");


                _requestURL = URLStringResult.Data ?? "";

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



        protected virtual IServiceResult<string> BuildServiceURL()
        {
            return _remoteServicesInfoService.BuildServiceURL(_remoteServiceName, _remoteServicePathName);
        }



        protected void InitializeHttpRequestMessage()
        {
            _requestMessage = new HttpRequestMessage { RequestUri = new Uri(_requestURL + "/" + _requestQuery) };
            _requestMessage.Method = _method;
            _requestMessage.Content = _content;
            _requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_mediaType));
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

    }
}
