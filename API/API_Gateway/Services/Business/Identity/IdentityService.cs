using API_Gateway.HttpServices.Identity.Interfaces;
using API_Gateway.Models;
using API_Gateway.Services.Business.Identity.Interfaces;
using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.Services.Business.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpIdentityService _httpIdentityService;

        public IdentityService(IHttpIdentityService httpIdentityService)
        {
            _httpIdentityService = httpIdentityService;
        }




        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {
            return await _httpIdentityService.Register(user);
        }



        public async Task<IServiceResult<string>> Login(UserToLoginDTO user)
        {
            return await _httpIdentityService.Login(user);
        }



        public async Task<IServiceResult<string>> AuthenticateService(string apiKey)
        {
            return await _httpIdentityService.AuthenticateService(apiKey);
        }

    }
}
