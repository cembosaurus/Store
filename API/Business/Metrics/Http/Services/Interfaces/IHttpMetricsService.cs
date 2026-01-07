using Business.Http.Services.Interfaces;
using Business.Metrics.DTOs;



namespace Business.Metrics.Http.Services.Interfaces
{
    public interface IHttpMetricsService : IHttpBaseService
    {
        Task UpdateAsync(MetricsCreateDTO metricsData, CancellationToken ct = default);
    }
}
