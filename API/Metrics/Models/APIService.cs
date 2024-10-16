namespace Metrics.Models
{
    public class APIService
    {
        public string Name { get; set; }
        //Id - for multiple K8 replicas of one service:
        public Guid Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public DateTime Deployed { get; set; }
        public DateTime Terminated { get; set; }

        public ICollection<HttpRequest> Requests { get; set; }
    }
}
