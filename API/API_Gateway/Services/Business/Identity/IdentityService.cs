using API_Gateway.Services.Business.Identity.Interfaces;
using Business.Identity.DTOs;
using Business.Identity.Http.Services.Interfaces;
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
            return await _httpIdentityService.Login_UserPassword(user);
        }



        public async Task<IServiceResult<string>> AuthenticateService(string apiKey)
        {
            return await _httpIdentityService.Login_ApiKey(apiKey);
        }

    }
}
