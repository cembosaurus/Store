using Business.Management.Appsettings.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;



namespace Business.Filters.Identity
{
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {

        private string _apiKey;


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var initResult = Initialize(context);

            if (!initResult.Item1)
            {
                context.Result = new UnauthorizedObjectResult(initResult.Item2);

                return;
            }


            if (!context.HttpContext.Request.Headers.TryGetValue("x-api-key", out var _secretApiKey))
            {

                if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var _JWT) && !string.IsNullOrWhiteSpace(_JWT))
                { 
                    context.Result = new UnauthorizedObjectResult("API-Key-Auth Filter: JWT-token authorization is not permited. Use API-Key instead !");
                
                    return;
                }

                context.Result = new UnauthorizedObjectResult("API-Key-Auth Filter: API-Key was not found in the request headers !");

                return;
            }


            if (!_apiKey.Equals(_secretApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API-Key-Auth Filter: API-Key does NOT match !");

                return;
            }


            await next();
        }


        private Tuple<bool, string> Initialize(ActionExecutingContext context)
        {
            var appsettingsService = context.HttpContext.RequestServices.GetService<IAppsettings_Provider>();

            if (appsettingsService == null)
            {
                return Tuple.Create(false, $"API-Key-Auth Filter: service 'AppsettingsService' was NOT found in Action Context !");
            }

            var apiKeyResult = appsettingsService.GetApiKey();


            if (!apiKeyResult.Status)
            {
                return Tuple.Create(false, $"API-Key-Auth Filter: Couldn't get API-Key from AppsettingsService. Reason: '{apiKeyResult.Message}'");
            }

            _apiKey = apiKeyResult.Data ?? "";

            return Tuple.Create(true, "");
        }
    }
}
