using Microsoft.AspNetCore.Http;

namespace Business.Middlewares
{
    public class Metrics_Server_MW
    {
        private RequestDelegate _next;


        public Metrics_Server_MW(RequestDelegate next)
        {
            _next = next;
        }



        public async Task Invoke(HttpContext context)
        {
            var v = context.Request.Headers["test_data"];
            var vv = context.Request.Headers.TryGetValue("test_data", out var whatever);

            context.Response.OnStarting(() =>
            {
                Console.WriteLine();

                return Task.CompletedTask;
            });

            await _next(context);

        }
    }
}
