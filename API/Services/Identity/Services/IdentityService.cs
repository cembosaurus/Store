using AutoMapper;
using Business.Identity.DTOs;
using Business.Identity.Enums;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.Models;
using Identity.Services.Interfaces;
using Identity.Services.JWT.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Models;
using System.Net.Http.Headers;



namespace Identity.Services
{
    public class IdentityService : IIdentityService
    {


        private readonly IServiceResultFactory _resultFact;
        private readonly IMapper _mapper;
        private readonly IHttpCartService _httpCartService;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public IdentityService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IServiceResultFactory resultFact, IMapper mapper, 
            IHttpCartService httpCartService, IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _resultFact = resultFact;
            _mapper = mapper;
            _httpCartService = httpCartService;
            _accessor = accessor;
        }




        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO userToRegister)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == userToRegister.Name.ToLower()))
                return _resultFact.Result<UserAuthDTO>(null, false, $"User name '{userToRegister.Name}' is not available !");

            var user = _mapper.Map<AppUser>(userToRegister);

            user.UserName = userToRegister.Name.ToLower();

            var result = await _userManager.CreateAsync(user, userToRegister.Password);


            if (!result.Succeeded)
                return _resultFact.Result<UserAuthDTO>(null, false, $"User '{user.UserName}' was NOT registered !");

            var roleResult = await _userManager.AddToRoleAsync(user, RoleType.Customer.ToString());


            if(!roleResult.Succeeded)
                return _resultFact.Result<UserAuthDTO>(null, false, $"User '{user.UserName}' was NOT assigneed with ROLE !");

            var userAuthDTO = _mapper.Map<UserAuthDTO>(user);


            var tokenResult = await _tokenService.CreateToken_ForUser(user);

            if (tokenResult == null || !tokenResult.Status)
                return _resultFact.Result<UserAuthDTO>(null, false, $"JWT token for User '{userToRegister.Name}' was NOT crerated !");

            userAuthDTO.Token = tokenResult.Data;

            _accessor.HttpContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userAuthDTO.Token).ToString();

            var cartCreateResult = await _httpCartService.CreateCart(user.Id);


            return _resultFact.Result(userAuthDTO, true, cartCreateResult == null || !cartCreateResult.Status ? $"Failed to create Cart for User '{user.UserName}' Id: '{user.Id}' !" : "");
        }



        public async Task<IServiceResult<string>> Login(UserToLoginDTO user)
        {
            var userFromRepo = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == user.Name.ToLower());

            if (userFromRepo == null)
                return _resultFact.Result<string>(null, false , $"User '{user.Name}' NOT found !");

            var result = await _signInManager.CheckPasswordSignInAsync(userFromRepo, user.Password, false);                 // 'false' = not locking out user if unsuccessful log in    

            if (!result.Succeeded)
                return _resultFact.Result<string>(null, false, $"User '{user.Name}' is NOT authorozed !");


            var tokenResult = await _tokenService.CreateToken_ForUser(userFromRepo);

            if (tokenResult == null || !tokenResult.Status || string.IsNullOrWhiteSpace(tokenResult.Data))
                return _resultFact.Result<string>(null, false, $"JWT token for User '{user.Name}' was NOT crerated !");

            return _resultFact.Result(tokenResult.Data, true);
        }



        public async Task<IServiceResult<string>> CreateTokenForService()
        {
            return await _tokenService.CreateToken_ForService();
        }




        public async Task AddRoles()
        {
            foreach (var role in Enum.GetValues(typeof(RoleType)))
            {
                await _roleManager.CreateAsync(new AppRole { Name = role.ToString() });     // TO DO: catch exception when SQL server is OFF !!!!!!!!
            }

        }



        public async Task AddDefaultUsers()
        {
            var admin = new AppUser { UserName = "Admin_1" };
            var manager = new AppUser { UserName = "Manager_1" };
            var user1 = new AppUser { UserName = "User_1" };
            var user2 = new AppUser { UserName = "User_2" };
            var user3 = new AppUser { UserName = "User_3" };

            await _userManager.CreateAsync(admin, "password");
            await _userManager.CreateAsync(manager, "password");
            await _userManager.CreateAsync(user1, "password");
            await _userManager.CreateAsync(user2, "password");
            await _userManager.CreateAsync(user3, "password");

            await _userManager.AddToRoleAsync(admin, RoleType.Admin.ToString());
            await _userManager.AddToRoleAsync(manager, RoleType.Manager.ToString());
            await _userManager.AddToRoleAsync(user1, RoleType.Customer.ToString());
            await _userManager.AddToRoleAsync(user2, RoleType.Customer.ToString());
            await _userManager.AddToRoleAsync(user3, RoleType.Customer.ToString());
        }


    }
}
