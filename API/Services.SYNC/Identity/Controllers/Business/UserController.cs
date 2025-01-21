using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Controllers.Business
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ClaimsPrincipal _user;
        private readonly int _principalId;


        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _user = httpContextAccessor.HttpContext.User;
            int.TryParse(_user.FindFirstValue(ClaimTypes.NameIdentifier), out _principalId);
        }




        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<object> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<object> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("name/{name}")]
        public async Task<object> GetUserByName(string name)
        {
            var result = await _userService.GetUserByName(name);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("withroles")]
        public async Task<object> GetAllUsersWithRoles()
        {
            var result = await _userService.GetAllUsersWithRoles();

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}/withroles")]
        public async Task<object> GetUserWithRoles(int id)
        {
            var result = await _userService.GetUserWithRoles(id);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}/changeroles")]
        public async Task<object> EditUserRoles(int id, [FromBody] string[] roles)
        {
            var result = await _userService.EditUserRoles(id, roles);

            return result;  // ctr res
        }



        [Authorize(Policy = "Everyone")]
        [HttpGet("current")]
        public async Task<object> GetCurrentUser()
        {
            var result = await _userService.GetUserById(_principalId);

            return result;  // ctr res
        }

    }
}
