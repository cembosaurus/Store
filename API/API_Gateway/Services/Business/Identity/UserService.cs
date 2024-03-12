using API_Gateway.HttpServices.Identity.Interfaces;
using API_Gateway.Services.Business.Identity.Interfaces;
using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;

namespace API_Gateway.Services.Business.Identity
{
    public class UserService : IUserService
    {

        private readonly IHttpUserService _httpUserService;


        public UserService(IHttpUserService httpUserService)
        {
            _httpUserService = httpUserService;
        }




        public async Task<IServiceResult<IEnumerable<string>>> EditUserRoles(int id, IEnumerable<string> roles)
        {
            return await _httpUserService.EditUserRoles(id, roles);
        }

        public async Task<IServiceResult<IEnumerable<UserReadDTO>>> GetAllUsers()
        {
            return await _httpUserService.GetAllUsers();
        }

        public async Task<IServiceResult<IEnumerable<UserWithRolesReadDTO>>> GetAllUsersWithRoles()
        {
            return await _httpUserService.GetAllUsersWithRoles();
        }

        public async Task<IServiceResult<UserReadDTO>> GetCurrentUser()
        {
            return await _httpUserService.GetCurrentUser();
        }

        public async Task<IServiceResult<UserReadDTO>> GetUserById(int id)
        {
            return await _httpUserService.GetUserById(id);
        }

        public async Task<IServiceResult<UserReadDTO>> GetUserByName(string name)
        {
            return await _httpUserService.GetUserByName(name);
        }

        public async Task<IServiceResult<UserWithRolesReadDTO>> GetUserWithRoles(int id)
        {
            return await _httpUserService.GetUserWithRoles(id);
        }
    }
}
