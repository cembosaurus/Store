using Inventory.Consumer.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Consumer.Data
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd, IConfiguration config)
        {
            // because it is static class it can't be registered. Create scope manualy:
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<InventoryContext>(), isProd, config);
            }
        }


        private static void SeedData(InventoryContext context, bool isProd, IConfiguration config)
        {
            // Apply migrations if it's in PRODUCTION env:
            if (isProd)
            {
                Console.WriteLine("--> Attempring to apply MIGRATIONS ...");

                try
                {
                    context.Database.Migrate();

                    Console.WriteLine($"--> MIGRATIONS - applied ...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run MIGRATIONS: {ex.Message}");
                }
            }


            string Photo(string photoId) => $"{config.GetSection("RemoteServices:StaticContentService").Value}/api/photos/{photoId}";

            if (!context.Items.Any())
            {
                Console.WriteLine("---> Seeding data ...");
                // Items:
                // IDs: 1 - 5:
                context.Items.AddRange(
                new Item { Name = "bird cage", Description = "for some birds", PhotoURL = Photo("bird_cage.jpg") },
                new Item { Name = "bulldozer", Description = "can go through wall", PhotoURL = Photo("bulldozer.jpg") },
                new Item { Name = "Marshall JCM800", Description = "rock'n'roll", PhotoURL = Photo("marshall_jcm800.jpg") },
                new Item { Name = "Fender Stratocaster", Description = "made SRV good", PhotoURL = Photo("fender_strat.jpg") },
                new Item { Name = "eggs", Description = "good for eating bad for ping pong", PhotoURL = Photo("eggs.jpg") }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> SEED was not performed. Data are already present.");
            }

        }

    }
}
