using AutoMapper;
using Business.Inventory.Http.Services.Interfaces;
using Business.Libraries.ServiceResult;
using Business.Libraries.ServiceResult.Interfaces;
using Identity.JWT;
using Identity.JWT.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Services.Identity.Models;

namespace Store.Test.Services.Identity.Services
{

    [TestFixture]
    internal class IdentityService_Test
    {

        private Mock<IMapper> _mapper;
        private Mock<IHttpCartService> _httpCartService;
        private Mock<IHttpContextAccessor> _accessor;
        private Mock<UserManager<AppUser>> _userManager;
        private Mock<RoleManager<AppRole>> _roleManager;
        private Mock<SignInManager<AppUser>> _signInManager;

        private IConfiguration _config;
        private IServiceResultFactory _resultFact;
        private IJWT_Provider _tokenService;


        [SetUp]
        public void Setup()
        { 
            _mapper = new Mock<IMapper>();
            _httpCartService = new Mock<IHttpCartService>();
            _accessor = new Mock<IHttpContextAccessor>();
            _userManager = new Mock<UserManager<AppUser>>();
            _roleManager = new Mock<RoleManager<AppRole>>();
            _signInManager = new Mock<SignInManager<AppUser>>();
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string> ( "", "" ),
                    new KeyValuePair<string, string> ( "", "" ),
                    new KeyValuePair<string, string> ("", "")
                })
                .Build();

            _resultFact = new ServiceResultFactory();

            _tokenService = new JWT_Provider(_config, new Mock<UserManager<AppUser>>().Object, _resultFact);
        
        }



        // Suitable for Integration Test instead of Unit Test




    }
}
