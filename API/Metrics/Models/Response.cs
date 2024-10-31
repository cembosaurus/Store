namespace Metrics.Models
{
    public class Response
    {
        // TransactionId - identifies main-request (client request):
        public int TransactionId { get; set; }
        // Index - identifies sub-response (response to app) in main-response (response to client):
        public int Index { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsIncoming { get; set; }



        // sender/receiver:
        public HttpMethod Method { get; set; }
        public Guid RemoteServiceId { get; set; }
        public string Path { get; set; }




        public HttpTransaction HttpTransaction { get; set; }
        public APIService APIService { get; set; }
        public HttpMethod HttpMethod { get; set; }
    }
}
