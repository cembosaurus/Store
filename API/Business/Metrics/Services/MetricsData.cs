using Business.Metrics.Services.Interfaces;
using Microsoft.Extensions.Primitives;




namespace Business.Metrics.Services
{
    public class MetricsData : IMetricsData
    {

        private List<KeyValuePair<string, StringValues>> _headers = new List<KeyValuePair<string, StringValues>>();
        private int _index;

        private static readonly Lock _lock = new();


        public MetricsData()
        {
            Initialize();
        }




        public void AddHeaders(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            lock (_lock)
            {
                if (headers != null && headers.Count() > 0)
                {
                    foreach (var header in headers)
                    {
                        _headers.Add(new KeyValuePair<string, StringValues>(header.Key, header.Value.ToArray()));
                    }
                }
            }
        }



        public void AddHeader(string key, string value)
        {
            lock (_lock)
            {
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                {
                    _headers.Add(new KeyValuePair<string, StringValues>(key, value));
                }
            }
        }



        public int Index
        {
            get { return _index; }
            set 
            { 
                lock (_lock)
                {
                    _index = value;
                }  
            }
        }



        public void Initialize()
        {
            _headers.Clear();
            _index = 0;
        }



        public IEnumerable<KeyValuePair<string, StringValues>> Headers
        {
            get { return _headers; }
        }



    }
}
