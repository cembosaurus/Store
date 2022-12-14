namespace Services.Ordering.Models
{
    public class Order
    {
        public Guid CartId { get; set; }
        public string OrderCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Dispatched { get; set; }


        public Cart Cart { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}
