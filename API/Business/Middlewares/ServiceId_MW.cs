﻿using Business.Management.Services.DTO;
using Microsoft.AspNetCore.Http;

namespace Business.Middlewares
{
    public class ServiceId_MW
    {
        private RequestDelegate _next;
        private readonly Guid _serviceId = Guid.NewGuid();
        // physical file-name of project as it's stated in f.e: docker file or file system, for acurate identification
        private readonly string _serviceName = Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "";

        public ServiceId_MW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // manual - GET response with data in body (Service Id, Name, ... etc):
            if (context.Request.Path == "/svcid")
            {
                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new ServiceIdReadDTO { Id = _serviceId, Name = _serviceName }));          
            }
            // auto - GET response with data in header (Service Id only):
            else
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("AppId", _serviceId.ToString());

                    return Task.CompletedTask;
                });

                await _next(context);

            }

        }
    }
}
