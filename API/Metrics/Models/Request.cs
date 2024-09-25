namespace Metrics.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Method { get; set; }
        public Guid ServiceId { get; set; }
        public string Path { get; set; }
        // request incoming or outgoing:
        public bool IsIncoming { get; set; }
        public DateTime Time { get; set; }

    }
}
