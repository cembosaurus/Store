using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Enums;
using Business.Management.Http.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Net;


namespace Business.Management.Http.Services
{
    public class HttpManagementService : HttpBaseService, IHttpManagementService
    {
        private readonly IHttpApiKeyAuthService _authService;
        private readonly IAppsettingsService _appsettingsService;
        private readonly IHttpAppClient _httpAppClient;
        private readonly IServiceResultFactory _resultFact;



        public HttpManagementService(IHttpApiKeyAuthService authService, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
            : base(httpAppClient)
        {
            _authService = authService;
            _appsettingsService = appsettingsService;
            _httpAppClient = httpAppClient;
            _resultFact = resultFact;
            _remoteServiceName = "ManagementService";
        }


        //------------------------------------------------------------------ EX: JWT is not available -- use API KEY !!!!!
        protected async override Task<HttpResponseMessage> Send()
        {
            var authResult = await AuthenticateWithApiKey();
            if (!authResult.Status || authResult.Data.IsNullOrEmpty())
                return _requestMessage.CreateErrorResponse(HttpStatusCode.NotFound, authResult.Message);

            var URLStringResult = BuildServiceURL();

            if (URLStringResult.Status)
                _requestURL = URLStringResult.Data ?? "";

            InitializeHttpRequestMessage();

            return await _httpAppClient.Send(_requestMessage);
        }


        protected override IServiceResult<string> BuildServiceURL()
        {
            var urlResult = _appsettingsService.GetRemoteServiceURL(_remoteServiceName);
            if(urlResult.Status)
                return _resultFact.Result("", false, urlResult.Message);

            var baseURL = urlResult.Data.GetUrl(TypeOfService.REST, false);
            if(baseURL.IsNullOrEmpty())
                return _resultFact.Result(baseURL, false, $"Base URL for remote service '{_remoteServiceName}' was NOT found in Appsettings !");

            var path = urlResult.Data.GetPathByName(_remoteServicePathName, TypeOfService.REST);
            if(path.IsNullOrEmpty())
                return _resultFact.Result(path, false, $"URL Path '{_remoteServicePathName}' for remote service '{_remoteServiceName}' was NOT found in Appsettings !");

            return _resultFact.Result(baseURL + "/" + path, true, "");
        }



        public async Task<IServiceResult<IEnumerable<ServiceURL>>> GetAllRemoteServicesURLs()
        {
            _remoteServicePathName = "RemoteService";

            _method = HttpMethod.Get;
            _requestQuery = $"{"url/all"}";

            var response = await Send();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<ServiceURL>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ServiceURL>>>(content);

            return result;
        }


        private async Task<IServiceResult<string>> AuthenticateWithApiKey()
        {
            return await _authService.LoginWithApiKey();
        }

    }
}
