﻿using Business.Exceptions.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;



namespace Business.Middlewares
{
    public class ErrorHandler_MW
    {

        private readonly RequestDelegate _next;
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
            catch (Exception error)
            {
                var response = context.Response;
                string result;

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

