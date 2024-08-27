using Microsoft.EntityFrameworkCore;
using Services.Identity.Data;
using Services.Identity.Models;

namespace Identity.Data
{
    public static class PrepDB
    {
        public static void RunMigrations(IApplicationBuilder app, bool isProd, IConfiguration config)
        {
            // because it is static class it can't be registered. Create scope manualy:
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                Migrate(serviceScope.ServiceProvider.GetService<IdentityContext>(), isProd, config);
            }
        }


        public static void PrepPopulation(IApplicationBuilder app)
        {
            // because it is static class it can't be registered. Create scope manualy:
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<IdentityContext>());
            }
        }



        private static void Migrate(IdentityContext context, bool isProd, IConfiguration config)
        {
            // Apply migrations if it's in PRODUCTION env:
            if (isProd)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;  
                Console.Write("Migrations:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" in progress ...");
                Console.ResetColor();

                try
                {
                    context.Database.Migrate();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Migrations:");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($" completed succesfuly.");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Migrations:");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"Fail: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{ex.Message}");                   
                    Console.ResetColor();
                }
            }
        }



        private static void SeedData(IdentityContext context)
        {
            if (!context.Address.Any())
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("DB Seed: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("in progress ...");
                Console.ResetColor();

                context.Address.AddRange(
                    new Address { City = "Sydney", Street = "Pitt", Number = 2 },
                    new Address { City = "Sydney", Street = "King", Number = 5 },
                    new Address { City = "Melbourne", Street = "Shrimp", Number = 7 },
                    new Address { City = "Terrey Hills", Street = "Kangaroo", Number = 123 },
                    new Address { City = "New York", Street = "Roastbeef", Number = 100 },
                    new Address { City = "Tokyo", Street = "Yakiniku", Number = 13 },
                    new Address { City = "Bratislava", Street = "Jozefa Kokota", Number = 542 },
                    new Address { City = "Roma", Street = "Via del Corso", Number = 1 },
                    new Address { City = "Paris", Street = "Champs-Élysées", Number = 1024 }
                    );

                context.SaveChanges();

                context.UserAddress.AddRange(
                    new UserAddress { UserId = 1, AddressId = 4 },
                    new UserAddress { UserId = 1, AddressId = 5 },
                    new UserAddress { UserId = 2, AddressId = 6 },
                    new UserAddress { UserId = 2, AddressId = 7 },
                    new UserAddress { UserId = 3, AddressId = 8 },
                    new UserAddress { UserId = 3, AddressId = 9 },
                    new UserAddress { UserId = 4, AddressId = 1 },
                    new UserAddress { UserId = 4, AddressId = 2 },
                    new UserAddress { UserId = 4, AddressId = 3 },
                    new UserAddress { UserId = 5, AddressId = 1 },
                    new UserAddress { UserId = 5, AddressId = 2 },
                    new UserAddress { UserId = 5, AddressId = 3 }
                    );

                context.SaveChanges();

                context.CurrentUsersAddress.AddRange(
                    new CurrentUsersAddress { UserId = 1, AddressId = 5 },
                    new CurrentUsersAddress { UserId = 2, AddressId = 7 },
                    new CurrentUsersAddress { UserId = 3, AddressId = 9 },
                    new CurrentUsersAddress { UserId = 4, AddressId = 3 },
                    new CurrentUsersAddress { UserId = 5, AddressId = 3 }
                    );

                var seedRerrsult =  context.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("DB Seed: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Success: {seedRerrsult} were saved into DB");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("DB Seed: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("was NOT run. DATA is already present in DB !");
                Console.ResetColor();
            }

        }

    }
}
