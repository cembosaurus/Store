using API_Gateway.Services.Identity.Interfaces;
using Business.Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API_Gateway.Controllers.Identity
{

    [Authorize]
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
        public async Task<IActionResult> Register(UserToRegisterDTO user)
        {
            var result = await _identityService.Register(user);

            return result.Status ? Ok(result) : BadRequest(result);
        }




        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDTO user)
        {
            var result = await _identityService.Login(user);

            return result.Status ? Ok(result) : BadRequest(result);
        }



        [AllowAnonymous]
        [HttpPost("service/authenticate")]
        public async Task<IActionResult> AuthenticateService(string apiKey)
        {
            var result = await _identityService.AuthenticateService(apiKey);

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
