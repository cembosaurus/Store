using Business.Management.Appsettings.Interfaces;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Business.Middlewares
{
    public class RequestHandler_MW
    {
        private readonly RequestDelegate _next;

        public RequestHandler_MW(RequestDelegate next)
        {
                _next = next;
        }



        public async Task InvokeAsync(HttpContext context, IGlobalConfig_PROVIDER globalConfig_Provider)
        {






            // nefunguje to ked deletnem v VS Code appsettings persistance. Data su poslane z Managementu ale niuesu potom prepisane v Ordering GC DB !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!






            // To Do: catch SQL error if page number out of range


            var gcResult = globalConfig_Provider.GetPersistence();
            var gcData = gcResult.Status && gcResult.Data!.Pagination.DefaultPageNumber > 0 && gcResult.Data.Pagination.DefaultPageSize > 0 ? gcResult.Data : new () { Pagination = new() { DefaultPageNumber = 90, DefaultPageSize = 3} };
            var defaultPageNumber = gcData!.Pagination.DefaultPageNumber;
            var defaultPageSize = gcData!.Pagination.DefaultPageSize;

            var page = context.Request.Query["page"] == StringValues.Empty ? defaultPageNumber.ToString() : context.Request.Query["page"][0];
            var size = context.Request.Query["size"] == StringValues.Empty ? defaultPageSize.ToString() : context.Request.Query["size"][0]; ;

            context.Items.Add("page", page);
            context.Items.Add("size", size);

            await _next(context);
        }



    }
}
