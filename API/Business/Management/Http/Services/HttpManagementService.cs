using Business.Http;
using Business.Http.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Http.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel;

namespace Business.Management.Http.Services
{
    public class HttpManagementService : HttpBaseService, IHttpManagementService
    {
        private readonly IHttpAppClient _httpAppClient;
        private readonly IServiceResultFactory _resultFact;



        public HttpManagementService(IHostingEnvironment env, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IServiceResultFactory resultFact)
            : base(env, appsettingsService, httpAppClient, resultFact)
        {
            _remoteServiceName = "ManagementService";
            _httpAppClient = httpAppClient;
            _resultFact = resultFact;           
        }





        protected async override Task<HttpResponseMessage> Send()
        {         
            return await _httpAppClient.Send(_requestMessage);
        }


        public async Task<IServiceResult<IEnumerable<Service_Model_AS>>> GetAllRemoteServices()
        {
            _remoteServicePathName = "RemoteService";

            _method = HttpMethod.Get;
            _requestQuery = $"{"url/all"}";

            var initResponse = InitializeRequest();

            if (!initResponse.Status)
                return _resultFact.Result<IEnumerable<Service_Model_AS>>(null, false, $"Request for remote service '{_remoteServiceName}' was NOT initialized ! Reason: {initResponse.Message}");

            var response = await Send();

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<IEnumerable<Service_Model_AS>>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<IEnumerable<Service_Model_AS>>>(content);

            return result;
        }

    }
}
