using Business.Filters.Identity;
using Business.Identity.DTOs;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers.Business
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






        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDTO userToRegister)
        {
            var result = await _identityService.Register(userToRegister);

            return Ok(result);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDTO user)
        {
            var result = await _identityService.Login(user);

            return Ok(result);
        }


    }
}
