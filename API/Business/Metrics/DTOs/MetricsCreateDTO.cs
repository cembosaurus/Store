

using Microsoft.Extensions.Primitives;

namespace Business.Metrics.DTOs
{
    public class MetricsCreateDTO
    {
        public IEnumerable<KeyValuePair<string, string[]>>? Data { get; set; }
    }
}
