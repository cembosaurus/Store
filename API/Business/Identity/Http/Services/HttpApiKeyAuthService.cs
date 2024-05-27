using Business.Identity.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;



namespace Business.Identity.Http.Services
{
    public class HttpApiKeyAuthService : IHttpApiKeyAuthService
    {

        private readonly IHttpIdentityService _httpIdentityService;
        private readonly IServiceResultFactory _resultFact;
        private readonly string? _apiKey;
        private readonly IHttpContextAccessor _accessor;
        private readonly IJWTTokenStore _jwtTokenStore;

        public HttpApiKeyAuthService(IConfiguration config, IServiceResultFactory resultFact, IHttpIdentityService httpIdentityService, IHttpContextAccessor accessor, IJWTTokenStore jwtTokenStore)
        {
            _httpIdentityService = httpIdentityService;
            _resultFact = resultFact;
            _apiKey = "KOKOT";// config.GetSection("Auth:ApiKey").Value ?? "";
            _accessor = accessor;
            _ = _accessor.HttpContext ?? new DefaultHttpContext();
            _jwtTokenStore = jwtTokenStore;
        }





        public async Task<IServiceResult<string>> LoginWithApiKey()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return _resultFact.Result("", true, "FAILED to read ApiKey from settings !");

            var response = await _httpIdentityService.AuthenticateService(_apiKey);

            if (response.Status)
            {
                _accessor.HttpContext?.Request.Headers.Add("Authorization", new AuthenticationHeaderValue("Bearer", response.Data).ToString());

                _jwtTokenStore.Token = response.Data;
            }

            return response;
        }






    }
}
