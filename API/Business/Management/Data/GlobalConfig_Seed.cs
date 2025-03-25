using Business.Enums;
using Business.Management.Services.Interfaces;
using Business.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;



namespace Business.Management.Data
{
    public static class GlobalConfig_Seed
    {
        public static async void Load(IApplicationBuilder app)
        {
            // set FALSE do disable HTTP GET to Management service:
            bool downloadGC = false;

            if (!downloadGC)
                return;

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IGlobalConfig_PROVIDER>();
                var cw = scope.ServiceProvider.GetRequiredService<ConsoleWriter>();

                try
                {
                    cw.Message("HTTP Get (outgoing)", "Global Config Seed", "App startup - Updating Global Config.", TypeOfInfo.INFO, "Waiting for response from Management API service...");

                    var result = await service.DownloadGlobalConfig();

                    cw.Message("HTTP Response (incoming)", "Global Config Seed", "Global Config update.", result.Status ? TypeOfInfo.SUCCESS : TypeOfInfo.WARNING, $"{(result.Status ? "" : result.Message)}");
                }
                catch (HttpRequestException ex)
                {
                    cw.Message("HTTP Response (incoming) ", "Global Config Seed", "Global Config update.", TypeOfInfo.FAIL, $"{ex.StatusCode}, {ex.Message}");
                }
            }

        }

    }

}
