using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
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

            builder.Services.AddTransient<Metrics_HttpClientRequest_INTERCEPTOR>();

            builder.Services.AddScoped<IMetricsData, MetricsData>();
            services.AddScoped<IHttpMetricsService, HttpMetricsService>();

            services.AddSingleton<IMetricsQueue>(_ => new ChannelMetricsQueue(capacity: 1000));
            services.AddHostedService<Metrics_Worker>();



            //IF http client is used instead of delegating handler interceptor:
            //builder.Services.AddHttpClient<IHttpClient_Metrics, HttpClient_Metrics>();
        }

    }
}
