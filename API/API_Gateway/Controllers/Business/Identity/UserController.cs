using API_Gateway.Services.Business.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API_Gateway.Controllers.Business.Identity
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly string _url;

        public UserController(IConfiguration conf, IUserService userService)
        {
            _url = conf.GetSection("RemoteServices:IdentityService:REST:BaseURL").Value + "/api/user";
            _userService = userService;

        }





        [Authorize(Policy = "Everyone")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            var result = await _userService.GetUserByName(name);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("withroles")]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var result = await _userService.GetAllUsersWithRoles();

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("{id}/withroles")]
        public async Task<IActionResult> GetUserWithRoles(int id)
        {
            var result = await _userService.GetUserWithRoles(id);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpPut("{id}/changeroles")]
        public async Task<IActionResult> EditUserRoles(int id, IEnumerable<string> roles)
        {
            var result = await _userService.EditUserRoles(id, roles);

            return result.Status ? Ok(result) : BadRequest(result);
        }


        [Authorize(Policy = "Everyone")]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _userService.GetCurrentUser();

            return result.Status ? Ok(result) : BadRequest(result);
        }
    }
}
