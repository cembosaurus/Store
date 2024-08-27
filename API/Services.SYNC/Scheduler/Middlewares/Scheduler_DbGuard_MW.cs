using Business.Data.Tools.Interfaces;
using Business.Middlewares;
using Microsoft.Data.SqlClient;
using Scheduler.StartUp.Interfaces;



namespace Scheduler.Middlewares
{
    public class Scheduler_DbGuard_MW : DbGuard_MW
    {

        public Scheduler_DbGuard_MW(RequestDelegate next, IGlobalVariables globalVariables)
            : base(next, globalVariables)
        {

        }



        public static async void HandleExpiredItems(WebApplication app)
        {

            using (var scope = app.Services.CreateScope())
            {
                var gv = app.Services.GetService<IGlobalVariables>();
                var service = scope.ServiceProvider.GetRequiredService<IRunAtStartUp>();             

                try
                {
                    await service.RemoveExpiredItemsFromCart();

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
