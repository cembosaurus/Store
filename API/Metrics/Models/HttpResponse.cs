namespace Metrics.Models
{
    public class HttpResponse
    {
        public Guid ServiceId { get; set; }
        // TransactionId - identifies main-request (client request):
        public int TransactionId { get; set; }
        // Index - identifies sub-response (response to app) in main-response (response to client):
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsIncoming { get; set; }



        // sender/receiver:
        public int MethodId { get; set; }
        public Guid RemoteServiceId { get; set; }
        public string Path { get; set; }




        public APIService Service { get; set; }
        public HttpMethod HttpMethod { get; set; }
    }
}
