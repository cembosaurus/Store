using Business.Http;
using Business.Http.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Business.Identity.Http.Services
{
    public class HttpIdentityService : HttpBaseService, IHttpIdentityService
    {

        private readonly IServiceResultFactory _resultFact;


        public HttpIdentityService(IHostingEnvironment env, IHttpAppClient httpAppClient, IRemoteServicesInfoService remoteServicesInfoService, IServiceResultFactory resultFact)
            : base(env, httpAppClient, remoteServicesInfoService)
        {
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {
            _method = HttpMethod.Post;
            _requestQuery = $"/identity/register";
            _content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), _encoding, _mediaType);

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
            _headers.Add("ApiKey", apiKey);

            var response = await Send();

            if (!response.IsSuccessStatusCode || response.Content.GetType().Name == "EmptyContent")
                return _resultFact.Result<string>(null, false, $"{(response.ReasonPhrase == "OK" ? "Fail" : response.ReasonPhrase)}: {response.RequestMessage?.Method}, {response.RequestMessage?.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<string>>(content);

            return result;
        }

    }
}
