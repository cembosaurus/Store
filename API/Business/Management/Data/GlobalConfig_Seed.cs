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

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IGlobalConfig_PROVIDER>();
                var message = scope.ServiceProvider.GetRequiredService<ConsoleWriter>();

                try
                {
                    message.Message("HTTP Get", "Global Config Seed", "App startup - Updating Global Config.", TypeOfInfo.INFO, "Waiting for response from Management API service...");

                    var result = await service.DownloadGlobalConfig();

                    message.Message("HTTP Response", "Global Config Seed", "Global Config update was not received !", TypeOfInfo.WARNING, $"{(result.Status ? "" : result.Message)}");
                }
                catch (HttpRequestException ex)
                {
                    message.Message("HTTP Response: ", "Global Config Seed", "Global Config update was not received ! ", TypeOfInfo.FAIL, $"{ex.StatusCode}, {ex.Message}");
                }
            }

        }

    }

}
