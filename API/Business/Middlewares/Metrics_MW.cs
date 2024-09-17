using Business.Metrics.Http.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
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
        private bool _metricsReporter;
        private int _index;
        private IHttpMetricsService _httpMetricsService;



        public Metrics_MW(RequestDelegate next, IConfiguration config)
        {
            _thisService = config.GetSection("Metrics:Name").Value ?? Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";
            _next = next;
        }





        public async Task Invoke(HttpContext context, IHttpMetricsService httpMetricsService)
        {

            _httpMetricsService = httpMetricsService;

            await AppId(context);

            await RequestHandler(context);

            await _next(context);

            Console.WriteLine($"------ OUT 2: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
        }




        private async Task RequestHandler(HttpContext context)
        {

            Request_IN(context);


            context.Response.OnStarting(async () =>
            {

                Response_OUT(context);




                //---------------------------------------------------------------------------------------------- To Do:

                // send data collected from whole chain of HTTP requests to Metrics API serevice:
                if (_metricsReporter)
                {
                    context.Response.Headers.Remove("Metrics.Index");

                    var metricsData = context.Response.Headers.Where(rh => rh.Key.StartsWith("Metrics.")).ToList();

                    if (_thisService == "MetricsService")
                    {
                        //// To Do: write data into DB --> Metrics API Service can't send data to itself via HTTP request !!!!

                        //Console.ForegroundColor = ConsoleColor.Cyan;
                        //Console.WriteLine($"{_thisService}: Updating metrics data localy... ");
                        //Console.ResetColor();
                    }
                    else 
                    {
                        //Console.ForegroundColor = ConsoleColor.Cyan;
                        //Console.Write("HTTP Post: ");
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        //Console.Write("Sending metrics data to ");
                        //Console.ForegroundColor = ConsoleColor.DarkCyan;
                        //Console.Write("Metrics ");
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        //Console.WriteLine("API service...");
                        //Console.ResetColor();

                        //var metricsHttpResult = await _httpMetricsService.Update(metricsData);//****************************************************** TRY to send data as string !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        //Console.ForegroundColor = ConsoleColor.Cyan;
                        //Console.Write("HTTP Response: ");
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        //Console.Write("from ");
                        //Console.ForegroundColor = ConsoleColor.DarkCyan;
                        //Console.Write("Metrics ");
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        //Console.Write("collector service ");
                        //Console.ForegroundColor = metricsHttpResult != null || metricsHttpResult.Status ? ConsoleColor.Cyan : ConsoleColor.Red;
                        //Console.Write(metricsHttpResult != null || metricsHttpResult.Status ? "Success: " : "Fail: ");
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        //Console.WriteLine(metricsHttpResult != null ? metricsHttpResult.Message : "Response not received !");
                        //Console.ResetColor();
                    }

                }
                //-------------------------------------------------------------------------------------------------------------------------------------------------




                Console.WriteLine($"------ OUT 1: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");

                return;// Task.CompletedTask;
            });


            Console.WriteLine($"------ OUT 2: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
        }



        private void Request_IN(HttpContext context)
        {
            // metrics START:

            _index = context.Request.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr) ? (int.TryParse(indexStrArr[0], out int indexInt) ? ++indexInt : 1) : 1;
            _metricsReporter = !context.Request.Headers.TryGetValue("Metrics.Reporter", out StringValues result);
            _requestFrom = context.Request.Headers.TryGetValue("Metrics.RequestFrom", out _requestFrom) ? _requestFrom[0] : "client_app";

            context.Request.Headers.Remove("Metrics.Index");
            context.Request.Headers.Add("Metrics.Index", _index.ToString());
            context.Request.Headers.Append("Metrics.Reporter", $"{_metricsReporter}");
            context.Response.Headers.Append($"Metrics.{_thisService}.{_appId}", $"{_index}.REQ.IN.{_requestFrom}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
        }



        private void Response_OUT(HttpContext context)
        {
            // metrics END:

            // increase index if HTTP response was not received:
            if (context.Response.StatusCode == 503)
                _index++;

            _index = context.Response.Headers.TryGetValue("Metrics.Index", out StringValues indexStrArr) ? (int.TryParse(indexStrArr[0], out int indexInt) ? ++indexInt : 0) : ++_index;

            context.Response.Headers.Remove("Metrics.Index");
            context.Response.Headers.Add("Metrics.Index", _index.ToString());
            context.Response.Headers.Append($"Metrics.{_thisService}.{_appId}", $"{_index}.RESP.OUT.{_requestFrom}.{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}");
        }



        private async Task AppId(HttpContext context)
        {
            if (context.Request.Path == "/appid")
                context.Response.Headers.Append($"AppId", $"{_thisService}.{_appId}");

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
