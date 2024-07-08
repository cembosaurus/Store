using Business.Data.Tools.Interfaces;
using Business.Middlewares;
using Microsoft.Data.SqlClient;
using Services.Inventory.Data;



namespace Inventory.Middlewares
{
    public class Inventory_DbGuard_MW : DbGuard_MW
    {

        public Inventory_DbGuard_MW(RequestDelegate next, IGlobalVariables globalVariables)
            : base(next, globalVariables)
        {

        }


        public static void Seed(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var gv = app.Services.GetService<IGlobalVariables>();

                try
                {
                    PrepDB.PrepPopulation(app, app.Environment.IsProduction());

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
