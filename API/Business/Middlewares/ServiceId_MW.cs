using Business.Data.Tools.Interfaces;
using Microsoft.AspNetCore.Http;



namespace Business.Middlewares
{
    public class ServiceId_MW
    {
        private RequestDelegate _next;
        private IGlobalVariables _gv;

        public ServiceId_MW(RequestDelegate next, IGlobalVariables gv)
        {
            _next = next;
            _gv = gv;
        }

        public async Task Invoke(HttpContext context)
        {
            // manual - GET response with data in body (Service Id, Name, ... etc):
            if (context.Request.Path == "/svcid")
            {
                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(_gv.ServiceID_Model));          
            }
            // auto - GET response with data in header (Service Id only):
            else
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Append($"ServiceId.{_gv.ServiceName}", _gv.ServiceID.ToString());

                    return Task.CompletedTask;
                });

                await _next(context);

            }

        }
    }
}
