
﻿using Business.Data.Tools.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Net;



namespace Business.Middlewares
{
    public class DbGuard_MW
    {

        private RequestDelegate _next;
        private IGlobalVariables _globalVariables;


        public DbGuard_MW(RequestDelegate next, IGlobalVariables globalVariables)
        {
            _next = next;
            _globalVariables = globalVariables;
        }


        public async Task Invoke(HttpContext context)
        {
            await CheckDBState(context);
        }




        private async Task CheckDBState(HttpContext context)
        {


            //
            //
            //
            //   *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *   temporary DISABLED. To Do: SQL server sends API request after being deployed to inform API that it is ready. Then ENABLE this code:  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *  *
            //
            //
            //
            //// If DB connection fails return HTTP 500:
            //if (!_globalVariables.DBState)
            //{
            //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //    context.Response.ContentType = "text/plain";
            //    await context.Response.WriteAsync($"HTTP Request refused ! DB Server is not connected!");
            //    return;
            //}

            await _next.Invoke(context);
        }


        protected static async void PrintFailMessage(SqlException ex)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("FAIL: ");
            Console.ForegroundColor = ConsoleColor.Yellow;

            switch (ex.Number)
            {
                case 11001:
                    Console.WriteLine($"DB was not connected! Reason: {ex.Message}");
                    break;
                default:
                    Console.WriteLine($"SQL Error! Reason: {ex.Message}");
                    break;
            }

            Console.ResetColor();
        }

    }
}
