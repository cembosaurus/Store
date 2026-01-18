using Business.Metrics.Http.Services;
using Business.Metrics.Http.Services.Interfaces;
using Business.Metrics.Services;
using Business.Metrics.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;



namespace Business.Metrics.DI
{
    public static class MetricsService_DI
    {
        public static IServiceCollection AddMetricsServiceIntegration(this IServiceCollection services)
        {
            services.AddTransient<Metrics_HttpClientRequest_INTERCEPTOR>();

            services.AddScoped<IMetricsData, MetricsData>();
            services.AddScoped<IHttpMetricsService, HttpMetricsService>();

            services.AddSingleton<IMetricsQueue>(_ => new ChannelMetricsQueue(capacity: 1000));
            services.AddHostedService<Metrics_Worker>();



            //IF http client is used instead of delegating handler interceptor:
            //builder.Services.AddHttpClient<IHttpClient_Metrics, HttpClient_Metrics>();

            return services;
        }

    }
}
