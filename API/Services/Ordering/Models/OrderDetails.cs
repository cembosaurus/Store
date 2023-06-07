namespace Services.Ordering.Models
{
    public class OrderDetails
    {
        public Guid CartId { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }


        public Order Order { get; set; }
    }
}
