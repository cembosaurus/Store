namespace Metrics.Models
{
    public class HttpResponse
    {
        public int TransactionId { get; set; }
        // Index - identifies sub-response (response to app) in main-response (response to client):
        public int Index { get; set; }
        public string Method { get; set; }
        public Guid ServiceId { get; set; }
        public string Path { get; set; }
        // response incoming or outgoing:
        public bool IsIncoming { get; set; }
        public DateTime Timestamp { get; set; }


        public HttpRequest Request { get; set; }
    }
}
