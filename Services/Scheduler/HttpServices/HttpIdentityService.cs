using Business.Identity.DTOs;
using Business.Identity.Http.Clients.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.JWT.Interfaces;
using Scheduler.HttpServices.Interfaces;

namespace Scheduler.HttpServices
{
    public class HttpIdentityService : IHttpIdentityService
    {

        private readonly IHttpIdentityClient _httpIdentityClient;
        private readonly IServiceResultFactory _resultFact;
        private readonly string _apiKey;
        private readonly IHttpContextAccessor _accessor;
        private readonly IJWTTokenStore _jwtTokenStore;

        public HttpIdentityService(IHttpIdentityClient httpIdentityClient, IServiceResultFactory resultFact, IConfiguration config, IHttpContextAccessor accessor, IJWTTokenStore jwtTokenStore)
        {
            _httpIdentityClient = httpIdentityClient;
            _resultFact = resultFact;
            _apiKey = config.GetSection("ApiKey").Value ?? "";
            _accessor = accessor;
            _ = _accessor.HttpContext ?? new DefaultHttpContext();
            _jwtTokenStore = jwtTokenStore;
        }


        //.......................... move AUTH, TOKEN and HttpContext handling into HttpServices




        public async Task<IServiceResult<UserAuthDTO>> Login(UserToLoginDTO user)
        {

            // To Do - if required

            return null;
        }



        public async Task<IServiceResult<UserAuthDTO>> Register(UserToRegisterDTO user)
        {

            // To Do - if required

            return null;
        }


    }
}
