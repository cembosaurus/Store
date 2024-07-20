using Business.Libraries.ServiceResult.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Business.Metrics.Http.Services.Interfaces
{
    public interface IHttpMetricsService
    {
        Task<IServiceResult<string>> Update(IEnumerable<KeyValuePair<string, StringValues>> metricsData);
    }
}
