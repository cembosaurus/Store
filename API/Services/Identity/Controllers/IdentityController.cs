using Business.Identity.DTOs;
using Identity.Filters;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Services.Identity.Controllers
{

    [Route("api/[controller]")]
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
        // Services authentication - 'ApiKey' value from request header
        [ApiKeyAuth]
        [HttpPost("service/authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var result = await _identityService.CreateTokenForService();

            return Ok(result);
        }
    }
}
