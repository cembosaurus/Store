using Business.Metrics.Services.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Business.Metrics.Services
{
    public sealed class MetricsData : IMetricsData
    {

        private readonly List<KeyValuePair<string, StringValues>> _headers = new();
        public int Index { get; set; }



        public MetricsData() => Initialize();




        public void Initialize()
        {
            _headers.Clear();
            Index = 0;
        }


        public void AddHeaders(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            if (headers == null) return;

            foreach (var h in headers)
                _headers.Add(new(h.Key, h.Value?.ToArray() ?? Array.Empty<string>()));
        }


        public void AddHeader(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                _headers.Add(new(key, value));
        }


        // Return a snapshot to avoid "collection modified" issues.
        public IEnumerable<KeyValuePair<string, StringValues>> Headers => _headers.ToArray();
    }
}
