using Business.Metrics.Services;
using Business.Metrics.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;



namespace Business.Metrics.DI
{
    public static class MetricsService_DI
    {
        public static void Register(WebApplicationBuilder builder)
        {
            var services = builder.Services;

            builder.Services.AddSingleton<IMetricsData, MetricsData>();
            builder.Services.AddScoped<Metrics_HttpClientRequest_INTERCEPTOR>();


            //IF http client is used instead of delegating handler interceptor:
            //builder.Services.AddHttpClient<IHttpClient_Metrics, HttpClient_Metrics>();
        }

    }
}
