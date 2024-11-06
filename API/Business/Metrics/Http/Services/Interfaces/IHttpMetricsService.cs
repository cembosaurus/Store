using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;
using Business.Metrics.DTOs;

namespace Business.Metrics.Http.Services.Interfaces
{
    public interface IHttpMetricsService : IHttpBaseService
    {
        Task<IServiceResult<string>> Update(MetricsCreateDTO metricsData);
    }
}
