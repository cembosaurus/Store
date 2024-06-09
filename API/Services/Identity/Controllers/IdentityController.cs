using Business.Filters.Identity;
using Business.Identity.DTOs;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Services.Identity.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IIdentityService _identityService;


        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }






        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDTO userToRegister)
        {
            var result = await _identityService.Register(userToRegister);

            return Ok(result);
        }



        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDTO user)
        {
            var result = await _identityService.Login(user);

            return Ok(result);
        }


        // Filter attr.:
        // Services authentication - 'ApiKey' value from request header. Returns JWT
        [ApiKeyAuth]
        [HttpPost("service/authenticate")]
        public async Task<IActionResult> LoginWithApiKey()
        {
            var result = await _identityService.CreateTokenForService();

            return Ok(result);
        }
    }
}
