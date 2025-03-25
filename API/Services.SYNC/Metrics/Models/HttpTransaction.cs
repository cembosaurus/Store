namespace Metrics.Models
{
    public class HttpTransaction
    {
        public int Id { get; set; }


        public ICollection<Request> Requests { get; set; }
        public ICollection<Response> Responses { get; set; }
    }
}
