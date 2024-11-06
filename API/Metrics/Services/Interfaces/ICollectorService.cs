using Business.Libraries.ServiceResult.Interfaces;
using Business.Metrics.DTOs;

namespace Metrics.Services.Interfaces
{
    public interface ICollectorService
    {
        Task<IServiceResult<bool>> AddHttpTransactionRecord(MetricsCreateDTO metricsData);
    }
}
