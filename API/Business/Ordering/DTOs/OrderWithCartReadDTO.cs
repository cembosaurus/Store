namespace Business.Ordering.DTOs
{
    public class OrderWithCartReadDTO
    {
        public string OrderId { get; set; }
        public DateTime Created { get; set; }
        public OrderDetailsReadDTO OrderDetails { get; set; }
        public CartReadDTO Cart { get; set; }
    }
}
