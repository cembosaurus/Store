using Business.Data.Tools.Interfaces;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;



namespace Services.Inventory.Data
{
    public static class PrepDB
    {

        public static void PrepPopulation(IApplicationBuilder app, bool isProd, IGlobalVariables globalVariables)
        {
            // because it is static class it can't be registered. Create scope manualy:
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<InventoryContext>(), isProd, globalVariables);
            }
        }




        private static void SeedData(InventoryContext context, bool isProd, IGlobalVariables globalVariables)
        {
            // Apply migrations if it's in PRODUCTION env:
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply MIGRATIONS ...");

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


            //string Photo(string photoId) => $"{config.GetSection("RemoteServices:StaticContentService").Value}/api/photos/{photoId}";
            string Photo(string photoId) => $"items/{photoId}";

            try
            {
                if (!context.CatalogueItems.Any())
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
                    // Catalogue Items:
                    // IDs: 6 - 10:
                    context.CatalogueItems.AddRange(
                        new CatalogueItem() { 
                            Item = new Item { 
                                Name = "car", 
                                Description = "AVD, 3.5l turbo, 7 seats",
                                PhotoURL = Photo("car.jpg")
                            },
                            ItemPrice = new ItemPrice { 
                                SalePrice = 123,
                                RRP = 125,
                                DiscountPercent = 10
                            },
                            Description = "For relaxing sunday trip with family and parents in law, or for transporting  friends to party",
                            Instock = 100
                        },
                        new CatalogueItem() { 
                            Item = new Item { 
                                Name = "onion", 
                                Description = "smells like onion",
                                PhotoURL = Photo("onion.jpg")
                            }, 
                            ItemPrice = new ItemPrice { 
                                SalePrice = 2.3,
                                RRP = 3.8,
                                DiscountPercent = 20
                            },
                            Description = "healthy stuff, prevents you from scorbut and other things that make you ill",
                            Instock = 100
                        },
                        new CatalogueItem() { 
                            Item = new Item { 
                                Name = "toothbrush", 
                                Description = "cleaning teeth",
                                PhotoURL = Photo("toothbrush.jpg")
                            }, 
                            ItemPrice = new ItemPrice { 
                                SalePrice = 12.8,
                                RRP = 15,
                                DiscountPercent = 10
                            },
                            Description = "you don't want to see goblin's smile every morning in mirror",
                            Instock = 100
                        },
                        new CatalogueItem() { 
                            Item = new Item { 
                                Name = "shoes", 
                                Description = "cover for your feet",
                                PhotoURL = Photo("shoes.jpg")
                            }, 
                            ItemPrice = new ItemPrice { 
                                SalePrice = 1000,
                                RRP = 1211,
                                DiscountPercent = 12
                            },
                            Description = "we have lowest prices on shoes. The larger foot the lower price",
                            Instock = 100
                        },
                        new CatalogueItem() { 
                            Item = new Item { 
                                Name = "paint", 
                                Description = "for wood, gibrock and bricks",
                                PhotoURL = Photo("paint.jpg")
                            }, 
                            ItemPrice = new ItemPrice { 
                                SalePrice = 1400,
                                RRP = 1500,
                                DiscountPercent = 20
                            },
                            Description = "can be used for covering fake paper carbord wall when you are selling your rotten house",
                            Instock = 100
                        },

                        // Accessory Items & Similar Product Items:
                        // IDs: 11 - 15:
                        new CatalogueItem() {
                            Item = new Item { 
                                Name = "wheel", 
                                Description = "so car can go",
                                PhotoURL = Photo("wheel.jpg")
                            },
                            ItemPrice = new ItemPrice { 
                                SalePrice = 300,
                                RRP = 350,
                                DiscountPercent = 10
                            },
                            Description = "short stock. Get some or you will regret it",
                            Instock = 100
                        },
                        new CatalogueItem() {
                            Item = new Item { 
                                Name = "steering wheel", 
                                Description = "so car can turn",
                                PhotoURL = Photo("steering_wheel.jpg")
                            },
                            ItemPrice = new ItemPrice { 
                                SalePrice = 2000,
                                RRP = 2100,
                                DiscountPercent = 20
                            },
                            Description = "Round, square, triangular, any shape for any personality",
                            Instock = 100
                        },
                        new CatalogueItem() {
                            Item = new Item { 
                                Name = "truck", 
                                Description = "big car",
                                PhotoURL = Photo("truck.jpg")
                            },
                            ItemPrice = new ItemPrice { 
                                SalePrice = 300000,
                                RRP = 350000,
                                DiscountPercent = 30
                            },
                            Description = "cab carry lots of things and go big distances",
                            Instock = 100
                        },
                        new CatalogueItem() {
                            Item = new Item { 
                                Name = "tomato", 
                                Description = "good for ya",
                                PhotoURL = Photo("tomato.jpg")
                            },
                            ItemPrice = new ItemPrice { 
                                SalePrice = 1.5,
                                RRP = 1.7,
                                DiscountPercent = 15
                            },
                            Description = "good for italian kitchen and juices",
                            Instock = 100
                        },
                        new CatalogueItem() {
                            Item = new Item { 
                                Name = "shoe laces", 
                                Description = "keep shoes on feet",
                                PhotoURL = Photo("shoe_laces.jpg")
                            },
                            ItemPrice = new ItemPrice { 
                                SalePrice = 10,
                                RRP = 13,
                                DiscountPercent = 10
                            },
                            Description = "if you don't want to fall on your nose tight them up",
                            Instock = 100
                        }
                    );


                    context.SaveChanges();

                    // Accessories:
                    context.Accessories.AddRange(
                        new AccessoryItem { ItemId = 6, AccessoryItemId = 11 },
                        new AccessoryItem { ItemId = 6, AccessoryItemId = 12 },
                        new AccessoryItem { ItemId = 9, AccessoryItemId = 15 }
                    );
                    // Similar Products:
                    context.SimilarProducts.AddRange(
                        new SimilarProductItem { ItemId = 6, SimilarProductItemId = 13 },
                        new SimilarProductItem { ItemId = 7, SimilarProductItemId = 14 }
                    );


                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("---> SEED was not performed ! Data is already present in DB.");
                }

                globalVariables.DBState = true;
            }
            catch
            {
                // alert exceptions handling middleware if DB is not connected, to redirect back all incoming HTTP requests:
                globalVariables.DBState = false;
            }


        }

    }
}
