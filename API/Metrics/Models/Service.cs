namespace Metrics.Models
{
    public class Service
    {
        public string Name { get; set; }
        //Id - for multiple replicas of one service:
        public Guid Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public DateTime ON { get; set; }
        public DateTime OFF { get; set; }



        public ICollection<Request> Request { get; set; }
    }
}
