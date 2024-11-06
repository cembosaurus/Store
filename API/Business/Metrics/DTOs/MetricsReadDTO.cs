using Metrics.Models;

namespace Business.Metrics.DTOs
{
    public class MetricsReadDTO
    {
        public HttpTransactionDTO? Transaction { get; set; }
        public HttpMethod? Method { get; set; }
        public HttpRequestDTO? Request { get; set; }
        public HttpResponseDTO? Response { get; set; }

    }
}
