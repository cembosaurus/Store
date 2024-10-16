namespace Metrics.Models
{
    public class HttpRequest
    {
        public int TransactionId { get; set; }
        // Index - identifies sub-request (app request) in main-request (client request):
        public int Index { get; set; }
        public string Method { get; set; }
        public Guid ServiceId { get; set; }
        public string Path { get; set; }
        // request incoming or outgoing:
        public bool IsIncoming { get; set; }
        public DateTime Timestamp { get; set; }


        public APIService Service { get; set; }
        public HttpResponse Response { get; set; }
    }
}
