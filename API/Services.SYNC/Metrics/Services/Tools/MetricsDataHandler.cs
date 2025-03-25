using Business.Libraries.ServiceResult.Interfaces;
using Metrics.Services.Tools.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Metrics.Services.Tools
{
    public class MetricsDataHandler : IMetricsDataHandler
    {

        private readonly IServiceResultFactory _resultFact;



        public MetricsDataHandler(IServiceResultFactory resultFact)
        {
            _resultFact = resultFact;
        }




        public IServiceResult<bool> Inspect(IEnumerable<KeyValuePair<string, string[]>> metricsData)
        {
            var result = new List<string>();

            foreach (var h in metricsData)
            {
                result.AddRange(h.Value.Select(v => h.Key.Replace("Metrics.", string.Empty) + "." + v));
            }









            return null;
        }



        

    }
}
