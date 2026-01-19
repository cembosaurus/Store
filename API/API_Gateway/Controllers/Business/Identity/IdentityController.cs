using API_Gateway.Services.Business.Identity.Interfaces;
using Business.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API_Gateway.Controllers.Business.Identity
{

    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : AppControllerBase
    {

        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }





        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<object> Register(UserToRegisterDTO user)
        {
            var result = await _identityService.Register(user);

            return result;  // ctr res
        }




        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<object> Login(UserToLoginDTO user)
        {
            var result = await _identityService.Login(user);

            return result;  // ctr res
        }



        // NOT USED. ApiKey is used to directly authenticate api service. No JWT necessary:
        [AllowAnonymous]
        [HttpPost("service/authenticate")]
        public async Task<object> AuthenticateService(string apiKey)
        {
            var result = await _identityService.AuthenticateService(apiKey);

            return result;  // ctr res
        }
    }
}
