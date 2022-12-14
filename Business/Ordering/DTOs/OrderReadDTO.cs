namespace Business.Ordering.DTOs
{
    public class OrderReadDTO
    {
        public Guid CartId { get; set; }
        public string OrderCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Dispatched { get; set; }
        public OrderDetailsReadDTO OrderDetails { get; set; }
    }
}
