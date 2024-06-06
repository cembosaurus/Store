using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Management.Appsettings.Interfaces;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;



namespace Business.Identity.Http.Services
{
    public class HttpApiKeyAuthService : IHttpApiKeyAuthService
    {
        private readonly IAppsettingsService _appsettingsService;
        private readonly IHttpIdentityService _httpIdentityService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IJWTTokenStore _jwtTokenStore;

        public HttpApiKeyAuthService(IAppsettingsService appsettingsService, IHttpIdentityService httpIdentityService, IHttpContextAccessor accessor, IJWTTokenStore jwtTokenStore)
        {
            _appsettingsService = appsettingsService;
            _httpIdentityService = httpIdentityService;
            _accessor = accessor;
            _ = _accessor.HttpContext ?? new DefaultHttpContext();
            _jwtTokenStore = jwtTokenStore;
        }





        public async Task<IServiceResult<string>> Authenticate()
        {
            var result = _appsettingsService.GetApiKey();

            if (!result.Status)
                return result;

            var response = await _httpIdentityService.Login_ApiKey(result.Data ?? "");

            if (response.Status)
            {
                _accessor.HttpContext?.Request.Headers.Add("Authorization", new AuthenticationHeaderValue("Bearer", response.Data).ToString());

                _jwtTokenStore.Token = response.Data ?? "";
            }

            return response;
        }






    }
}
