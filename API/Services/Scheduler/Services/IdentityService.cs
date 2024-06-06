using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Scheduler.Services.Interfaces;

namespace Scheduler.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly IServiceResultFactory _resultFact;
        private readonly IHttpApiKeyAuthService _httpApiKeyAuthService;

        public IdentityService(IServiceResultFactory resultFact, IHttpApiKeyAuthService httpApiKeyAuthService)
        {
            _resultFact = resultFact;
            _httpApiKeyAuthService = httpApiKeyAuthService;
        }




        public async Task<IServiceResult<string>> AuthenticateService()
        {
            Console.WriteLine($"--> AUTHENTICATING the Scheduler service with Api Key ....");


            var authResult = await _httpApiKeyAuthService.Authenticate();

            if (authResult == null || !authResult.Status)
                return _resultFact.Result("", false, authResult?.Message ?? "Authentication FAILED !");
      

            return _resultFact.Result(authResult.Data, true, authResult.Message);
        }


    }
}
