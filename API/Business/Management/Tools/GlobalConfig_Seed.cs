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
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Updating Global Config. Waiting for response from Management service...");
                    Console.ResetColor();
                    var result = await service.ReLoad();
                    Console.ForegroundColor = result.Status ? ConsoleColor.Cyan : ConsoleColor.Red;
                    Console.Write($"{(result.Status ? "Success: " : $"Failed: ")}");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{(result.Status ? "Response from Management service received." : result.Message)}");
                    Console.ResetColor();
                }
                catch (HttpRequestException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FAIL: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Global config was NOT received from Management service ! EX: {ex.Message}");
                    Console.ResetColor();
                }
            }

        }

    }

}
