using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;



namespace Business.Middlewares
{
    public class Metrics_MW
    {
        private static readonly Guid _appId = Guid.NewGuid();
        private static readonly DateTime _deployed = DateTime.UtcNow;
        private static readonly AppId_MODEL _appId_Model = new AppId_MODEL { AppId = _appId, Deployed = _deployed };

        private RequestDelegate _next;
        private readonly string _thisService;
        private StringValues _requestFrom;
        private bool _metricsDataSender;



        public Metrics_MW(RequestDelegate next, IConfiguration config)
        {
            _thisService = config.GetSection("Name").Value ?? Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _next = next;
        }




        public async Task Invoke(HttpContext context)
        {
            await AppId(context);

            await RequestHandler(context);

            await _next(context);
        }




        private async Task RequestHandler(HttpContext context)
        {
            var _timeIn = DateTime.UtcNow;

            _metricsDataSender = !context.Request.Headers.TryGetValue("Metrics.DataSender", out StringValues result);

            _requestFrom = context.Request.Headers.TryGetValue("Metrics.RequestFrom", out _requestFrom) ? _requestFrom[0] : "client_app";

            context.Request.Headers.Append($"Metrics.DataSender", $"{_metricsDataSender}");

            context.Response.Headers.Append($"Metrics.{_thisService}.{_appId}", $"REQ.IN.{_requestFrom}.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");


            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append($"Metrics.{_thisService}.{_appId}", $"RESP.OUT.{_requestFrom}.{_timeIn.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");




                //---------------------------------------------------------------------------------------------- To Do:

                // send data collected from whole chain of HTTP requests to Metrics API serevice:
                if (_metricsDataSender)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"------------------- {_thisService} -------------------------- SENDING METRICS ");
                    Console.ResetColor();
                }
                //-------------------------------------------------------------------------------------------------------------------------------------------------






                return Task.CompletedTask;
            });

        }


        private async Task AppId(HttpContext context)
        {
            if (context.Request.Path == "/appid")
            {
                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(_appId_Model));
            }
            else
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Append($"AppId", $"{_thisService}.{_appId}");

                    return Task.CompletedTask;
                });

            }
        }





        public static AppId_MODEL AppId_Model
        {
            get { return _appId_Model; }
        }






        public class AppId_MODEL
        {
            public Guid AppId = Guid.Empty;
            public string AppName = "";
            public string ServiceName = "";
            public DateTime Deployed;

        }


    }

}
