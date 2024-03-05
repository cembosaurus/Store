using Business.Identity.Http.Services.Interfaces;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace Business.Middlewares
{
    public class ApiKeyAuth_MW
    {
        private RequestDelegate _next;

        public ApiKeyAuth_MW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHttpApiKeyAuthService httpApiKeyAuthService, IJWTTokenStore tokenStore)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Headers[HeaderNames.Authorization].ToString()) &&
                (string.IsNullOrWhiteSpace(tokenStore.Token) || tokenStore.IsExipred))
            {
                var authResult = await httpApiKeyAuthService.AuthenticateService();

                if (authResult == null || !authResult.Status)
                    Console.WriteLine($"--> Service FAILED to authenticate ! {(authResult == null ? string.Empty : "Reason: '" + authResult.Message)}'");
            }

            await _next(context);
        }

    }
}
