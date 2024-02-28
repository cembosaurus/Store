using Business.Identity.Http.Services.Interfaces;
using Business.Scheduler.JWT.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Middlewares
{
    public class Metrics
    {
        private RequestDelegate _next;
        private DateTime _timeIn;

        public Metrics(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _timeIn= DateTime.UtcNow;

           
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add("TimeIn", _timeIn.ToString());
                context.Response.Headers.Add("TimeOut", DateTime.UtcNow.ToString());

                return Task.CompletedTask;
            });

            await _next(context);

        }
    }
}
