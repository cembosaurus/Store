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
                    Console.Write($"Updating Global Config. Waiting for response from ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"Management ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"service...");
                    Console.ResetColor();

                    var result = await service.ReLoad();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"HTTP Response: ");
                    Console.ForegroundColor = ConsoleColor.Yellow ;
                    Console.Write($"from ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"Management ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"service ");
                    Console.ForegroundColor = result.Status ? ConsoleColor.Cyan : ConsoleColor.Red;
                    Console.Write($"{(result.Status ? " received. " : $"Failed ! : ")}");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{(result.Status ? "" : result.Message)}");
                    Console.ResetColor();
                }
                catch (HttpRequestException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"HTTP Response: ");
                    Console.Write($"from ");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write($"Management ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"service ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Failed ! : ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Global Config update was not received ! ");
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
