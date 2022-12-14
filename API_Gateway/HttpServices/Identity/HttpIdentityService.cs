using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using API_Gateway.Models;
using API_Gateway.HttpServices.Identity.Interfaces;

namespace API_Gateway.HttpServices.Identity
{
    public class HttpIdentityService : IHttpIdentityService
    {
        private readonly IHttpIdentityClient _httpIdentityClient;

        public HttpIdentityService(IHttpIdentityClient httpIdentityClient)
        {
            _httpIdentityClient = httpIdentityClient;
        }





        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {
            var response = await _httpIdentityClient.Register(user);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<UserAuthDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<UserAuthDTO>> Login(UserToLoginDTO user)
        {
            var response = await _httpIdentityClient.Login(user);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<UserAuthDTO>>(content);

            return result;
        }



        public async Task<IServiceResult<string>> AuthenticateService(string apiKey)
        {
            var response = await _httpIdentityClient.AuthenticateService(apiKey);

            var content = response.Content.ReadAsStringAsync().Result;

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceResult<string>>(content);

            return result;
        }

    }
}
