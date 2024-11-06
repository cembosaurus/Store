using Business.Libraries.ServiceResult.Interfaces;

namespace Metrics.Services.Tools.Interfaces
{
    public interface IMetricsDataHandler
    {
        IServiceResult<bool> Inspect(IEnumerable<KeyValuePair<string, string[]>> metricsData);
    }
}
