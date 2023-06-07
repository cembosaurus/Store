using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Net;
using System.Text.Json;

namespace Business.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                string result;

                response.ContentType = "application/json";

                switch (error)
                {
                    case HttpRequestException ex:

                        var w32ex = error as Win32Exception;

                        if (w32ex == null)
                            w32ex = error.InnerException as Win32Exception;

                        if (w32ex != null)
                        {
                            int code = w32ex.ErrorCode;

                            // winsock connection error:
                            context.Response.StatusCode =
                                code == 10061
                                ? (int)HttpStatusCode.ServiceUnavailable
                                : response.StatusCode;

                            Console.WriteLine($"--> Could NOT get response !");
                        }

                        break;

                    default:
                        // unhandled error:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        break;
                }

                // If status code is not in defined range:
                context.Response.StatusCode =
                    context.Response.StatusCode < 100 || context.Response.StatusCode > 599
                    ? (int)HttpStatusCode.InternalServerError
                    : context.Response.StatusCode;

                result = JsonSerializer.Serialize(new { message = 
                    $"{error?.Source} / " +
                    $"{context.Response.StatusCode} / " +
                    $"{error?.Message}" });

                Console.WriteLine($"--> {result}");

                await response.WriteAsync(result);
            }
        }
    }
}

