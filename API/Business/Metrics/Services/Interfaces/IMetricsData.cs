using Microsoft.Extensions.Primitives;



namespace Business.Metrics.Services.Interfaces
{
    public interface IMetricsData
    {
        IEnumerable<KeyValuePair<string, StringValues>> Headers { get; }
        int Index { get; set; }

        void AddHeader(string key, string value);
        void AddHeaders(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers);
        void Initialize();
    }
}
