﻿using Business.Identity.DTOs;
using Identity.Services.Interfaces;
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
        public async Task<object> Register(UserToRegisterDTO userToRegister)
        {
            var result = await _identityService.Register(userToRegister);

            return (ActionResult)result;
        }



        [HttpPost("login")]
        public async Task<object> Login(UserToLoginDTO user)
        {
            var result = await _identityService.Login(user);

            return (ActionResult)result;
        }


    }
}
