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
            if(!Initialize(context))
            {
                context.Result = new UnauthorizedResult();

                return;
            }


            if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var _secretApiKey))
            { 
                context.Result = new UnauthorizedResult();

                return;
            }


            if (!_apiKey.Equals(_secretApiKey))
            {
                context.Result = new UnauthorizedResult();

                return;
            }


            await next();
        }


        private bool Initialize(ActionExecutingContext context)
        {
            var appsettingsService = context.HttpContext.RequestServices.GetService<IAppsettingsService>();

            if (appsettingsService == null)
            {
                Console.WriteLine($"--> API-Key-Auth Filter: 'AppsettingsService' was NOT located in filter !");

                return false;
            }

            var apiKeyResult = appsettingsService.GetApiKey();

            if (apiKeyResult == null)
            {
                Console.WriteLine($"--> API-Key-Auth Filter: API-Key was not retrieved from 'AppsettingsService' !");

                return false;
            }

            if (!apiKeyResult.Status)
            {
                Console.WriteLine($"--> API-Key-Auth Filter --> AppsettingsService: {apiKeyResult.Message}");

                return false;
            }

            _apiKey = apiKeyResult.Data;

            return true;
        }
    }
}
