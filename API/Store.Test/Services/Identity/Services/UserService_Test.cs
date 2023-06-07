using AutoMapper;
using Business.Identity.DTOs;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.Data.Repositories.Interfaces;
using Identity.Models;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using Services.Identity.Models;
using System.Security.Claims;

namespace Store.Test.Services.Identity.Services
{

    [TestFixture]
    internal class UserService_Test
    {
        private IUserService _userService;

        private Mock<IUserRepository> _userRepo = new Mock<IUserRepository>();
        private Mock<IMapper> _mapper = new Mock<IMapper>();
        private Mock<UserManager<AppUser>> _userManager;
        private Mock<RoleManager<AppRole>> _roleManager;

        private AppUser _user1, _user2, _user3;
        private readonly int _user1_Id = 1, _user2_Id = 2, _user3_Id = 3;
        private UserReadDTO _user1DTO, _user2DTO, _user3DTO;
        private List<AppUser> _users;
        private IEnumerable<UserReadDTO> _userDTOList;
        private IEnumerable<string> _roles = new List<string> { "role1", "role2", "role3", "role4" };
        private IEnumerable<AppRole> _appRoles;
        private IEnumerable<string> _newRoles = new List<string> { "non existing role 1", "role4" };
        private AppUserRole _user1Role1, _user2Role1, _user2Role2, _user3Role1, _user3Role2, _user3Role3;



        [SetUp]
        public void Setup()
        {
            _appRoles = new List<AppRole> 
            { 
                new AppRole { Name = _roles.ElementAt(0) }, 
                new AppRole { Name = _roles.ElementAt(1) }, 
                new AppRole { Name = _roles.ElementAt(2) },
                new AppRole { Name = _roles.ElementAt(3) }
            };
            _user1Role1 = new AppUserRole { RoleId = 1, UserId = _user1_Id, AppRole = _appRoles.ElementAt(0) };
            _user2Role1 = new AppUserRole { RoleId = 1, UserId = _user1_Id, AppRole = _appRoles.ElementAt(0) };
            _user2Role2 = new AppUserRole { RoleId = 2, UserId = _user1_Id, AppRole = _appRoles.ElementAt(1) };
            _user3Role1 = new AppUserRole { RoleId = 1, UserId = _user1_Id, AppRole = _appRoles.ElementAt(0) };
            _user3Role2 = new AppUserRole { RoleId = 2, UserId = _user1_Id, AppRole = _appRoles.ElementAt(1) };
            _user3Role3 = new AppUserRole { RoleId = 3, UserId = _user1_Id, AppRole = _appRoles.ElementAt(2) };

            _user1 = new AppUser { Id = _user1_Id, UserName = "User Name 1", UserRoles = new List<AppUserRole> { _user1Role1 }};
            _user2 = new AppUser { Id = _user2_Id, UserName = "User Name 2", UserRoles = new List<AppUserRole> { _user2Role1, _user2Role2 }};
            _user3 = new AppUser { Id = _user3_Id, UserName = "User Name 3", UserRoles = new List<AppUserRole> { _user3Role1, _user3Role2, _user3Role3 }};
            _users = new List<AppUser> { _user1, _user2, _user3 };
            _user1DTO = new UserReadDTO { Id = _user1.Id, Name = _user1.UserName };
            _user2DTO = new UserReadDTO { Id = _user2.Id, Name = _user2.UserName };
            _user3DTO = new UserReadDTO { Id = _user3.Id, Name = _user3.UserName };
            _userDTOList = new List<UserReadDTO> { _user1DTO, _user2DTO, _user3DTO};

            var userStore = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            _userManager.Object.UserValidators.Add(new UserValidator<AppUser>());
            _userManager.Object.PasswordValidators.Add(new PasswordValidator<AppUser>());
            _userManager.Setup(x => x.DeleteAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<AppUser, string>((x, y) => _users.Add(x));
            _userManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);

            var _signInManager = new Mock<SignInManager<AppUser>>(_userManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(), null, null, null, null);
            var roleStore = new Mock<IRoleStore<AppRole>>();
            _roleManager = new Mock<RoleManager<AppRole>>(
                         roleStore.Object, null, null, null, null);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(o => o.HttpContext.User).Returns(It.IsAny<ClaimsPrincipal>());

            _userService = new UserService(_userRepo.Object, new ServiceResultFactory(), _mapper.Object, _userManager.Object, _roleManager.Object);
        }




        //  GetAllUsers()

        [Test]
        public void GetAllUsers_WhenCalled_ReturnsListOfUsers()
        {
            _userRepo.Setup(ur => ur.GetAllUsers()).Returns(Task.FromResult(_users.AsEnumerable()));

            _mapper.Setup(m => m.Map<IEnumerable<UserReadDTO>>(It.IsAny<IEnumerable<AppUser>>())).Returns(_userDTOList);


            var result = _userService.GetAllUsers().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_userDTOList.Count()));
            Assert.That(result.Data.ElementAt(0), Is.EqualTo(_userDTOList.ElementAt(0)));
        }


        [Test]
        public void GetAllUsers_NoUsersFound_ReturnsMessage()
        {
            _userRepo.Setup(ur => ur.GetAllUsers()).Returns(Task.FromResult(new List<AppUser>().AsEnumerable()));

            _mapper.Setup(m => m.Map<IEnumerable<UserReadDTO>>(It.IsAny<IEnumerable<AppUser>>())).Returns(new List<UserReadDTO>().AsEnumerable());


            var result = _userService.GetAllUsers().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo("NO users found !"));
        }



        //  GetUserById()

        [Test]
        public void GetUserById_WhenCalled_ReturnsUser()
        {
            _userRepo.Setup(ur => ur.GetUserById(It.IsAny<int>())).Returns(Task.FromResult(_user1));

            _mapper.Setup(m => m.Map<UserReadDTO>(It.IsAny<AppUser>())).Returns(_user1DTO);


            var result = _userService.GetUserById(_user1.Id).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Id, Is.EqualTo(_user1DTO.Id));
        }


        [Test]
        public void GetUserById_NoUserFound_ReturnsMessage()
        {
            _userRepo.Setup(ur => ur.GetUserById(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<AppUser>()));


            var result = _userService.GetUserById(_user1.Id).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"User '{_user1.Id}' was NOT found !"));
        }



        //  GetUserByName()

        [Test]
        public void GetUserByName_WhenCalled_ReturnsUser()
        {
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(_user1));

            _mapper.Setup(m => m.Map<UserReadDTO>(It.IsAny<AppUser>())).Returns(_user1DTO);


            var result = _userService.GetUserByName(_user1.UserName).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Id, Is.EqualTo(_user1DTO.Id));
        }


        [Test]
        public void GetUserByName_NoUserFound_ReturnsMessage()
        {
            _userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<AppUser>()));


            var result = _userService.GetUserByName(_user1.UserName).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo($"User '{_user1.UserName}' was NOT found !"));
        }



        // GetAllUsersWithRoles()

        [Test]
        public void GetAllUsersWithRoles_WhenCalled_ReturnsAllUsersWithRoles()
        {
            _userManager.Setup(um => um.Users).Returns(_users.AsQueryable());


            var result = _userService.GetAllUsersWithRoles().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(_userDTOList.Count()));
            Assert.That(result.Data.ElementAt(0).Name, Is.EqualTo(_user1.UserName));
            Assert.That(result.Data.ElementAt(2).Roles.ElementAt(2), Is.EqualTo(_roles.ElementAt(2)));
        }


        [Test]
        public void GetAllUsersWithRoles_NoUserFound_ReturnsMessage()
        {
            _userManager.Setup(um => um.Users).Returns(new List<AppUser>().AsQueryable());


            var result = _userService.GetAllUsersWithRoles().Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo("NO users found !"));

        }



        //  GetUserWithRoles()

        [Test]
        public void GetUserWithRoles_WhenCalled_ReturnsAllUsersWithRoles()
        {
            _userManager.Setup(um => um.Users).Returns(_users.AsQueryable());


            var result = _userService.GetUserWithRoles(_user3_Id).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Name, Is.EqualTo(_user3.UserName));
            Assert.That(result.Data.Roles.ElementAt(2), Is.EqualTo(_roles.ElementAt(2)));
        }


        [Test]
        public void GetUserWithRoles_NoUserFound_ReturnsMessage()
        {
            _userManager.Setup(um => um.Users).Returns(new List<AppUser>().AsQueryable());


            var result = _userService.GetUserWithRoles(_user3_Id).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Message, Is.EqualTo("User NOT found !"));

        }



        //  EditUserRoles()

        [Test]
        public void EditUserRoles_WhenCalled_UpdateUsersRoles()
        {
            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(_user1));

            _userManager.Setup(um => um.GetRolesAsync(It.IsAny<AppUser>())).Returns(Task.FromResult((IList<string>)_roles));

            _userManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            _userManager.Setup(um => um.AddToRolesAsync(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            _roleManager.Setup(rm => rm.Roles).Returns(_appRoles.AsQueryable());


            var result = _userService.EditUserRoles(_user1_Id, _newRoles).Result;


            Assert.IsTrue(result.Status);
            Assert.That(result.Data.Count(), Is.EqualTo(1));
            Assert.That(result.Data.ElementAt(0), Is.EqualTo(_newRoles.ElementAt(1)));
        }


        [Test]
        public void EditUserRoles_UserNotFound_ReturnsFailMessage()
        {
            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(It.IsAny<AppUser>()));


            var result = _userService.EditUserRoles(_user1_Id, _newRoles).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"User with ID: '{_user1_Id}' NOT found !"));
        }


        [Test]
        public void EditUserRoles_RolesNotFound_ReturnsFailMessage()
        {
            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(_user1));

            _roleManager.Setup(rm => rm.Roles).Returns(new List<AppRole>().AsQueryable());



            var result = _userService.EditUserRoles(_user1_Id, _newRoles).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo("Entered roles DO NOT exist !"));
        }


        [Test]
        public void EditUserRoles_RolesNotRemoved_ReturnsFailMessage()
        {
            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(_user1));

            _userManager.Setup(um => um.GetRolesAsync(It.IsAny<AppUser>())).Returns(Task.FromResult((IList<string>)_roles));

            _userManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError())));


            _roleManager.Setup(rm => rm.Roles).Returns(_appRoles.AsQueryable());


            var result = _userService.EditUserRoles(_user1_Id, _newRoles).Result;


            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Failed to REMOVE user: '{_user1.UserName}' from roles !"));
        }


        [Test]
        public void EditUserRoles_RolesNotAdded_ReturnsFailMessage()
        {
            _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(_user1));

            _userManager.Setup(um => um.GetRolesAsync(It.IsAny<AppUser>())).Returns(Task.FromResult((IList<string>)_roles));

            _userManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            _userManager.Setup(um => um.AddToRolesAsync(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError())));

            _roleManager.Setup(rm => rm.Roles).Returns(_appRoles.AsQueryable());


            var result = _userService.EditUserRoles(_user1_Id, _newRoles).Result;

            Assert.IsFalse(result.Status);
            Assert.That(result.Message, Is.EqualTo($"Failed to ADD user: '{_user1.UserName}' to roles !"));
        }


    }
}
