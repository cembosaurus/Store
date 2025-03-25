namespace Metrics.Models
{
    public class Method
    {
        public string Name { get; set; }


        public ICollection<Request> Requests { get; set; }
        public ICollection<Response> Responses { get; set; }
    }
}
