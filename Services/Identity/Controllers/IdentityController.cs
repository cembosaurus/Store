using Business.Identity.DTOs;
using Identity.Filters;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Services.Identity.Controllers
{

    //[Authorize]       // No need to authorize for Register and Login:
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

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDTO user)
        {
            var result = await _identityService.Login(user);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        // Filter attr.:
        // Services authentication - 'ApiKey' value from request header
        [StoreApiKeyAuth]
        [HttpPost("service/authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var result = await _identityService.CreateTokenForService();

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
