namespace Metrics.Models
{
    public class Response
    {
        public int RequestId { get; set; }
        // Index - identifies sub-response (response to app) in main-response (response to client):
        public int Index { get; set; }
        public string Method { get; set; }
        public Guid ServiceId { get; set; }
        public string Path { get; set; }
        // response incoming or outgoing:
        public bool IsIncoming { get; set; }
        public DateTime Timestamp { get; set; }


        public Request Request { get; set; }
    }
}
