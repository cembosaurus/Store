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
using System.Linq.Expressions;
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


        private int _pageNumber = 1;
        private int _pageSize = 20;


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

            // check query string:
            if (int.TryParse(context.Request.Query["page"], out int page)
                && int.TryParse(context.Request.Query["size"], out int size))
            {
                _pageNumber = page; 
                _pageSize = size;

                AddQueryString();

                return;
            }


            // try get data from GC provider:
            var gcResult = globalConfig_Provider.GetPersistence();
            
            var pg = gcResult.Status
                ? gcResult.Data!.Pagination
                : new()
                {
                    DefaultPageNumber = _pageNumber,
                    DefaultPageSize = _pageSize
                };
            
            _pageNumber = pg.DefaultPageNumber;
            _pageSize = pg.DefaultPageSize;

            AddQueryString();



            void AddQueryString()
            {

                var queryItems = context.Request.Query
                .Where(i => i.Key != "page" && i.Key != "size")
                .Select(x => { return new KeyValuePair<string, string>(x.Key, x.Value[0] ?? ""); })
                .ToList();

                queryItems.Add(new KeyValuePair<string, string>("page", ((_pageNumber - 1) * _pageSize).ToString()));
                queryItems.Add(new KeyValuePair<string, string>("size", _pageSize.ToString()));

                var builder = new QueryBuilder(queryItems);

                context.Request.QueryString = builder.ToQueryString();
            }

        }


    }
}
