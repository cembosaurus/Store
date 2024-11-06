namespace Business.Metrics.DTOs
{
    public class APIServiceDTO
    {
        public string Name { get; set; }
        //Id - for multiple K8 replicas of one service:
        public Guid Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public DateTime Deployed { get; set; }
        public DateTime Terminated { get; set; }

        public ICollection<Request> Requests { get; set; }
        public ICollection<Response> Responses { get; set; }
    }
}
