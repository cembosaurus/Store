using Business.Data.Tools.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;



namespace Business.Middlewares
{
    public class AppId_MW
    {
        
        private RequestDelegate _next;
        private IGlobalVariables _gv;
        private readonly string _serviceName;



        public AppId_MW(RequestDelegate next, IConfiguration config, IGlobalVariables gv)
        {
            _gv = gv;
            _serviceName = config.GetSection("Name").Value ?? Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _next = next;
        }



        public async Task Invoke(HttpContext context)
        {
            // manual - GET response with data in body (Service Id, Name, ... etc):
            if (context.Request.Path == "/appid")
            {
                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(_gv.AppID_Model));          
            }
            // auto - GET response with data in header (Service Id only):
            else
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Append($"AppId.{_serviceName}", _gv.AppID.ToString());

                    return Task.CompletedTask;
                });

                await _next(context);

            }

        }
    }
}
