using Business.Management.Appsettings.Interfaces;
using Business.Management.Services;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Business.AMQP.Custom.RequestMethods;




namespace Business.Middlewares
{
    public class RequestHandler_MW
    {
        private readonly RequestDelegate _next;


        private const int _defaultPageNumber = 1;
        private const int _defaultPageSize = 20;


        public RequestHandler_MW(RequestDelegate next)
        {
                _next = next;
        }



        public async Task InvokeAsync(HttpContext context, IGlobalConfig_PROVIDER globalConfig_Provider)
        {

            Pagging(context, globalConfig_Provider);

            await _next(context);
        }




        private void Pagging(HttpContext context, IGlobalConfig_PROVIDER globalConfig_Provider)
        {

            // nefunguje to ked deletnem v VS Code appsettings persistance. Data su poslane z Managementu ale niuesu potom prepisane v Ordering GC DB !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // !!!! order runs alobe -> default OK. After starting Management -> Order GC NOT updated!!!!!!






            // To Do: catch SQL error if page number out of range


            // Get pagination from request:
            var pageStr = context.Request.Query["page"] == StringValues.Empty ? _defaultPageNumber.ToString() : context.Request.Query["page"][0];
            var sizeStr = context.Request.Query["size"] == StringValues.Empty ? _defaultPageSize.ToString() : context.Request.Query["size"][0];  
            
            if (!(int.TryParse(pageStr, out var pageInt) && pageInt > 0
                && int.TryParse(sizeStr, out var sizeInt) && sizeInt > 0))
            {
                // If pagination not found in request, get pagging from GC:
                var gcResult = globalConfig_Provider.GetPersistence();
                var gcData = gcResult.Status && gcResult.Data!.Pagination.DefaultPageNumber > 0 && gcResult.Data.Pagination.DefaultPageSize > 0
                    ? gcResult.Data
                    : new()
                    {
                        Pagination = new()
                        {
                            DefaultPageNumber = _defaultPageNumber,
                            DefaultPageSize = _defaultPageSize
                        }
                    };

                pageInt = gcData!.Pagination.DefaultPageNumber;
                sizeInt = gcData!.Pagination.DefaultPageSize;
            }

            // insert pagination into query string:
            var queryItems = context.Request.Query.Where(i => i.Key != "page" && i.Key != "size").Select(x => { return new KeyValuePair<string, string>(x.Key, x.Value[0] ?? ""); }).ToList();

            queryItems.Add(new KeyValuePair<string, string>("page", ((pageInt - 1) * sizeInt).ToString()));
            queryItems.Add(new KeyValuePair<string, string>("size", sizeInt.ToString()));

            var builder = new QueryBuilder(queryItems);

            context.Request.QueryString = builder.ToQueryString();
        }


    }
}
