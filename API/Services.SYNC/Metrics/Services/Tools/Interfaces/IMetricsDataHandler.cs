using Business.Libraries.ServiceResult.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Metrics.Services.Tools.Interfaces
{
    public interface IMetricsDataHandler
    {
        IServiceResult<bool> Inspect(IEnumerable<KeyValuePair<string, string[]>> metricsData);
    }
}
