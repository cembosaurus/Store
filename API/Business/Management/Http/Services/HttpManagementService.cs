using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.Net;


namespace Business.Management.Http.Services
{
    public class HttpManagementService : HttpBaseService, IHttpManagementService
    {
        private readonly IHttpAppClient _httpAppClient;
        private readonly IServiceResultFactory _resultFact;



        public HttpManagementService(IHostingEnvironment env, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
            : base(env, appsettingsService, httpAppClient)
        {
            _httpAppClient = httpAppClient;
            _resultFact = resultFact;
            _remoteServiceName = "ManagementService";

            Initialize();
        }



        protected async override Task<HttpResponseMessage> Send()
        {
            var result = AddApiKeyToHeader();

            if (!result)
                return _requestMessage.CreateErrorResponse(HttpStatusCode.Unauthorized, "API-Key was NOT found in settings !");

            InitializeHttpRequestMessage();

            return await _httpAppClient.Send(_requestMessage);
        }



        public async Task<IServiceResult<IEnumerable<ServiceURL_AS>>> GetAllRemoteServicesURLs()
        {
            _remoteServicePathName = "RemoteService";

            _method = HttpMethod.Get;
            _requestQuery = $"{"url/all"}";

            var response = await Send();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<ServiceURL_AS>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<ServiceURL_AS>>>(content);

            return result;
        }

    }
}
