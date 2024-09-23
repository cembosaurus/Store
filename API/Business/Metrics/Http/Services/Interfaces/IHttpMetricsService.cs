using Business.Http.Services.Interfaces;
using Business.Libraries.ServiceResult.Interfaces;



namespace Business.Metrics.Http.Services.Interfaces
{
    public interface IHttpMetricsService : IHttpBaseService
    {
        Task<IServiceResult<string>> Update(IEnumerable<KeyValuePair<string, string[]>> metricsData);
    }
}
