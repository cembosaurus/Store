using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Filters
{
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {

        private const string _headerApiKeyName = "ApiKey";



        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (!context.HttpContext.Request.Headers.TryGetValue(_headerApiKeyName, out var _secretApiKey))
            { 
                context.Result = new UnauthorizedResult();

                return;
            }


            var conf = context.HttpContext.RequestServices.GetService<IConfiguration>();

            var apiKey = conf.GetSection("ApiKey").Value;


            if (!apiKey.Equals(_secretApiKey))
            {
                context.Result = new UnauthorizedResult();

                return;
            }


            await next();
        }
    }
}
