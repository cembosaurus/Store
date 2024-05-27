using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Business.Identity.Http.Services
{
    public class HttpIdentityService : HttpBaseService, IHttpIdentityService
    {

        private readonly IServiceResultFactory _resultFact;


        public HttpIdentityService(IHostingEnvironment env, IAppsettingsService appsettingsService, IHttpAppClient httpAppClient, IRemoteServicesInfo_Provider remoteServicesInfoService, IServiceResultFactory resultFact)
            : base(env, appsettingsService, httpAppClient, remoteServicesInfoService, resultFact)
        {
            _remoteServiceName = "IdentityService";
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/identity/register";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            var initResponse = InitializeRequest();

            if (!initResponse.Status)
                return _resultFact.Result<UserAuthDTO>(null, false, $"Request for remote service '{_remoteServiceName}' was NOT initialized ! Reason: {initResponse.Message}");

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<UserAuthDTO>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<UserAuthDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<string>> Login(UserToLoginDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/identity/login";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

            var initResponse = InitializeRequest();

            if (!initResponse.Status)
                return _resultFact.Result<string>(null, false, $"Request for remote service '{_remoteServiceName}' was NOT initialized ! Reason: {initResponse.Message}");

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<string>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<string>>(content);

            return result;
        }



        public async Task<IServiceResult<string>> AuthenticateService(string apiKey)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/identity/service/authenticate";
            _requestHeaders.Add("ApiKey", apiKey);

            var initResponse = InitializeRequest();

            if (!initResponse.Status)
                return _resultFact.Result<string>(null, false, $"Request for remote service '{_remoteServiceName}' was NOT initialized ! Reason: {initResponse.Message}");

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<string>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<string>>(content);

            return result;
        }

    }
}
