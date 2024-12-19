using Business.Http.Services.Interfaces;
using Business.Metrics.DTOs;



namespace Business.Metrics.Http.Services.Interfaces
{
    public interface IHttpMetricsService : IHttpBaseService
    {
        void Update(MetricsCreateDTO metricsData);
    }
}
