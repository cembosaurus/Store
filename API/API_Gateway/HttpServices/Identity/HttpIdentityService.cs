using API_Gateway.HttpServices.Identity.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.HttpServices.Identity
{
    public class HttpIdentityService : IHttpIdentityService
    {
        private readonly IHttpIdentityClient _httpIdentityClient;
        private readonly IServiceResultFactory _resultFact;

        public HttpIdentityService(IHttpIdentityClient httpIdentityClient, IServiceResultFactory resultFact)
        {
            _httpIdentityClient = httpIdentityClient;
            _resultFact = resultFact;
        }





        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {
            var response = await _httpIdentityClient.Register(user);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<UserAuthDTO>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<UserAuthDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<string>> Login(UserToLoginDTO user)
        {
            var response = await _httpIdentityClient.Login(user);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<string>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<string>>(content);

            return result;
        }



        public async Task<IServiceResult<string>> AuthenticateService(string apiKey)
        {
            var response = await _httpIdentityClient.AuthenticateService(apiKey);

            if (!response.IsSuccessStatusCode)
                return _resultFact.Result<string>(null, false, $"{response.ReasonPhrase}: {response.RequestMessage.Method}, {response.RequestMessage.RequestUri}");

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<string>>(content);

            return result;
        }

    }
}
