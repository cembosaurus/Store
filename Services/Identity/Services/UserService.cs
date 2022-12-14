using AutoMapper;
using Business.Identity.DTOs;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.Data.Repositories.Interfaces;
using Identity.Models;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Models;
using System.Linq;

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
            Console.WriteLine($"--> GETTING all users ......");


            var users = await _repo.GetAllUsers();

            if (!users.Any())
                return _resultFact.Result<IEnumerable<UserReadDTO>>(null, false, "NO users found !");

            return _resultFact.Result(_mapper.Map<IEnumerable<UserReadDTO>>(users), true);
        }




        public async Task<IServiceResult<UserReadDTO>> GetUserById(int id)
        {
            Console.WriteLine($"--> GETTING user '{id}' ......");


            var user = await _repo.GetUserById(id);

            if (user == null)
                return _resultFact.Result<UserReadDTO>(null, false, $"User '{id}' was NOT found !");

            return _resultFact.Result(_mapper.Map<UserReadDTO>(user), true);
        }




        public async Task<IServiceResult<UserReadDTO>> GetUserByName(string name)
        {
            Console.WriteLine($"--> GETTING user '{name}' ......");


            var user = await _userManager.FindByNameAsync(name);

            if (user == null)
                return _resultFact.Result<UserReadDTO>(null, false, $"User '{name}' was NOT found !");

            return _resultFact.Result(_mapper.Map<UserReadDTO>(user), true);
        }




        public async Task<IServiceResult<IEnumerable<UserWithRolesReadDTO>>> GetAllUsersWithRoles()
        {
            Console.WriteLine($"--> GETTING users with roles ......");


            var users = await _userManager.Users
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
                .ToListAsync();


            if (!users.Any())
                return _resultFact.Result<IEnumerable<UserWithRolesReadDTO>>(null, false, "NO users found !");

            return _resultFact.Result(_mapper.Map<IEnumerable<UserWithRolesReadDTO>>(users), true);
        }



        public async Task<IServiceResult<UserWithRolesReadDTO>> GetUserWithRoles(int id)
        {
            Console.WriteLine($"--> GETTING users with roles ......");


            var user = await _userManager.Users
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
                .SingleOrDefaultAsync();


            if (user == null)
                return _resultFact.Result<UserWithRolesReadDTO>(null, false, "User NOT found !");

            return _resultFact.Result(_mapper.Map<UserWithRolesReadDTO>(user), true);
        }



        public async Task<IServiceResult<IEnumerable<string>>> EditUserRoles(int id, IEnumerable<string> roles)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return _resultFact.Result<IEnumerable<string>>(null, false, "User NOT found !");
            if (!roles.Any())
                return _resultFact.Result<IEnumerable<string>>(null, false, "Matching roles NOT found !");


            Console.WriteLine($"--> EDITING roles for user '{user.Id}': '{user.UserName}' ......");


            var appRoles = _roleManager.Roles.Select(ar => ar.Name.ToString()).ToList();

            var rolesToUpdate = roles.Intersect(appRoles).ToList();

            var userRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (!removeResult.Succeeded)
                return _resultFact.Result<IEnumerable<string>>(null, false, $"Failed to REMOVE user: '{user.UserName}' from roles !");

            var addResult = await _userManager.AddToRolesAsync(user, rolesToUpdate);

            if (!addResult.Succeeded)
                return _resultFact.Result<IEnumerable<string>>(null, false, $"Failed to ADD user: '{user.UserName}' to roles !");

            return _resultFact.Result(_mapper.Map<IEnumerable<string>>(rolesToUpdate), true, $"{(rolesToUpdate.Count() < roles.Count() ? roles.Count() - rolesToUpdate.Count() : "")} NON matching roles were ignored !");
        }

    }
}
