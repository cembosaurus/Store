﻿namespace Metrics.Models
{
    public class Request
    {
        // TransactionId - identifies main-request (client request):
        public int TransactionId { get; set; }
        // Index - identifies sub-request (app request) in main-request (client request):
        public int Index { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsIncoming { get; set; }



        // sender/receiver:
        public string HttpMethodName { get; set; }
        public Guid RemoteServiceId { get; set; }
        public string Path { get; set; }




        public HttpTransaction HttpTransaction { get; set; }
        public APIService APIService { get; set; }
        public Method Method { get; set; }
    }
}
