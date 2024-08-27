using Business.Data.Tools.Interfaces;
using Business.Middlewares;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;



namespace Ordering.Middlewares
{
    public class Ordering_DbGuard_MW : DbGuard_MW
    {


        public Ordering_DbGuard_MW(RequestDelegate next, IGlobalVariables globalVariables)
            : base(next, globalVariables)
        {

        }




        public static void Migrate_DB(WebApplication app)
        {
            if (app.Environment.IsProduction())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var gv = app.Services.GetService<IGlobalVariables>();
                    var db = scope.ServiceProvider.GetRequiredService<OrderingContext>();

                    try
                    {
                        db.Database.Migrate();

                        gv.DBState = true;
                    }
                    catch (SqlException ex)
                    {
                        PrintFailMessage(ex);

                        gv.DBState = false;
                    }
                }
            }
        }

    }
}
