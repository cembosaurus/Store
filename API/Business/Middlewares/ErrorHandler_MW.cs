using Business.Exceptions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace Business.Middlewares
{
    public class ErrorHandler_MW
    {

        private readonly RequestDelegate _next;
        private HttpResponse response;
        private string result;
        private readonly IExId _exId;



        public ErrorHandler_MW(RequestDelegate next, IExId exId)
        {
            _next = next;
            _exId = exId;
        }



        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SqlException sqlError)
            {
                response = context.Response;

                response.ContentType = "application/json";

                switch (sqlError.ErrorCode)
                {
                    case -2146232060:
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                await CreateMessage(sqlError.Source!, context.Response.StatusCode, sqlError.Message);

                await response.WriteAsync(result);
            }
            catch (Exception error)
            {
                response = context.Response;

                response.ContentType = "application/json";

                switch (error)
                {
                    case HttpRequestException ex:

                        context.Response.StatusCode = _exId.Http_503(error)
                            ? (int)HttpStatusCode.ServiceUnavailable
                            : response.StatusCode;

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

                context.Response.StatusCode =
                    context.Response.StatusCode == 400
                    ? (int)HttpStatusCode.BadRequest
                    : context.Response.StatusCode;


                await CreateMessage(error.Source!, context.Response.StatusCode, error.Message);


                await response.WriteAsync(result);
            }
        }



        private async Task CreateMessage(string source = "", int statusCode = 0, string message = "")
        {
            result = JsonSerializer.Serialize(new{
                message =
                $"{source} / " +
                $"{statusCode} / " +
                $"{message}"
            });

            Console.WriteLine($"--> {result}");
        }

    }

}

