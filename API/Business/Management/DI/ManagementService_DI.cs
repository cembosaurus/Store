using Business.Management.Appsettings;
using Business.Management.Appsettings.Interfaces;
using Business.Management.Appsettings.Models;
using Business.Management.Data;
using Business.Management.Http.Services;
using Business.Management.Http.Services.Interfaces;
using Business.Management.Services;
using Business.Management.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;



namespace Business.Management.DI
{
    public static class ManagementService_DI
    {
        public static void Register(WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddSingleton<Config_Global_DB>();
            services.AddScoped<IConfig_Global_REPO, Config_Global_REPO>();
            services.AddScoped<IGlobalConfig_PROVIDER, GlobalConfig_PROVIDER>();
            services.AddTransient<IAppsettings_PROVIDER, Appsettings_PROVIDER>();
            services.Configure<Config_Global_AS_MODEL>(builder.Configuration.GetSection("Config.Global"));
            services.AddScoped<IHttpManagementService, HttpManagementService>();
            builder.Services.AddScoped<Management_HttpClientRequest_INTERCEPTOR>();

            //var config = builder.Configuration;
            //var prodConnStr = config.GetSection("ConnectionStrings:InventoryConnStr:Prod").Value;
            //if (!string.IsNullOrEmpty(prodConnStr))
            //{
            //    prodConnStr = prodConnStr
            //        .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"))
            //        .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
            //        .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
            //        .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));

            //    builder.Configuration["ConnectionStrings:InventoryConnStr:Prod"] = prodConnStr;
            //}

        }
    }
}
