using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Management.Tools
{
    public static class GlobalConfig_Seed
    {
        public static async void Load(IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IGlobalConfig_PROVIDER>();

                try
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"HTTP Get: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Updating Global Config. Waiting for response from Management service...");
                    Console.ResetColor();

                    var result = await service.ReLoad();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"HTTP Response: ");
                    Console.ForegroundColor = result.Status ? ConsoleColor.Cyan : ConsoleColor.Red;
                    Console.Write($"{(result.Status ? "" : $"Failed !")}");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{(result.Status ? "Global Config update: GET Response from Management service was received." : result.Message)}");
                    Console.ResetColor();
                }
                catch (HttpRequestException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"HTTP Response: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAIL: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Global Config update: GET Response from Management API service was not received ! ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"EX: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{ex.Message}");
                    Console.ResetColor();
                }
            }

        }

    }

}
