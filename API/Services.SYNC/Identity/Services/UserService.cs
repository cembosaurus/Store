using AutoMapper;
using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.Data.Repositories.Interfaces;
using Identity.Models;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Models;

namespace Identity.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repo;
        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(IUserRepository repo, IServiceResultFactory resultFact, IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _repo = repo;
            _resultFact = resultFact;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }





        public async Task<IServiceResult<IEnumerable<UserReadDTO>>> GetAllUsers()
        {
            var users = await _repo.GetAllUsers();

            return _resultFact.Result(_mapper.Map<IEnumerable<UserReadDTO>>(users), true, users.Any() ? "" : "NO users found !");
        }




        public async Task<IServiceResult<UserReadDTO>> GetUserById(int id)
        {
            var user = await _repo.GetUserById(id);

            return _resultFact.Result(_mapper.Map<UserReadDTO>(user), true, user != null ? "" : $"User '{id}' was NOT found !");
        }




        public async Task<IServiceResult<UserReadDTO>> GetUserByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);

            return _resultFact.Result(_mapper.Map<UserReadDTO>(user), true, user != null ? "" : $"User '{name}' was NOT found !");
        }




        public async Task<IServiceResult<IEnumerable<UserWithRolesReadDTO>>> GetAllUsersWithRoles()
        {
            var users = _userManager.Users.AsQueryable();

            if (!users.Any())
                return _resultFact.Result<IEnumerable<UserWithRolesReadDTO>>(null, true, "NO users found !");


            var usersWithRoles = users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.AppRole)
                .OrderBy(u => u.UserName)
                .Select(u => new UserWithRolesReadDTO
                {
                    Id = u.Id,
                    Name = u.UserName,
                    Roles = u.UserRoles
                    .Select(r => r.AppRole.Name)
                    .ToList()
                })
                .ToList();


            return _resultFact.Result(usersWithRoles.AsEnumerable(), true);
        }



        public async Task<IServiceResult<UserWithRolesReadDTO>> GetUserWithRoles(int id)
        {
            var users = _userManager.Users.AsQueryable();

            if (!users.Any())
                return _resultFact.Result<UserWithRolesReadDTO>(null, true, "User NOT found !");


            var userWithRoles = users
                .Where(u => u.Id == id)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.AppRole)
                .Select(u => new UserWithRolesReadDTO
                {
                    Id = u.Id,
                    Name = u.UserName,
                    Roles = u.UserRoles
                    .Select(r => r.AppRole.Name)
                    .ToList()
                })
                .SingleOrDefault();


            return _resultFact.Result(userWithRoles, true);
        }



        public async Task<IServiceResult<IEnumerable<string>>> EditUserRoles(int id, IEnumerable<string> roles)
        {
            //var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return _resultFact.Result<IEnumerable<string>>(null, false, $"User with ID: '{id}' NOT found !");

            var allExistingRoles = _roleManager.Roles.Select(ar => ar.Name.ToString()).ToList();

            if (!allExistingRoles.Any())
                return _resultFact.Result<IEnumerable<string>>(null, false, "Entered roles DO NOT exist !");

            var existingRolesToUpdate = roles.Intersect(allExistingRoles).ToList();

            var userRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (!removeResult.Succeeded)
                return _resultFact.Result<IEnumerable<string>>(null, false, $"Failed to REMOVE user: '{user.UserName}' from roles !");

            var addResult = await _userManager.AddToRolesAsync(user, existingRolesToUpdate);

            if (!addResult.Succeeded)
                return _resultFact.Result<IEnumerable<string>>(null, false, $"Failed to ADD user: '{user.UserName}' to roles !");

            return _resultFact.Result(existingRolesToUpdate.AsEnumerable(), true, $"{(existingRolesToUpdate.Count() < roles.Count() ? roles.Count() - existingRolesToUpdate.Count() : "")} NON matching roles were ignored !");
        }

    }
}
