using Business.Data.Tools.Interfaces;
using Business.Middlewares;
using Identity.Data;
using Identity.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace Identity.Middlewares
{
    public class Identity_DbGuard_MW : DbGuard_MW
    {

        public Identity_DbGuard_MW(RequestDelegate next, IGlobalVariables globalVariables)
            : base(next, globalVariables)
        {

        }



        public static async void Migrate_Prep_Seed_DB(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var gv = app.Services.GetService<IGlobalVariables>();
                var service = scope.ServiceProvider.GetRequiredService<IIdentityService>();

                try
                {
                    // Migrations:
                    PrepDB.RunMigrations(app, app.Environment.IsProduction(), app.Configuration);

                    // Seed roles:
                    await service.AddRoles();

                    // Create admin and manager:
                    await service.AddDefaultUsers();

                    // Seed DB:
                    PrepDB.PrepPopulation(app);

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
