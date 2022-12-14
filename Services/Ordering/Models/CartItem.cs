namespace Services.Ordering.Models
{
    public class CartItem
    {
        public Guid CartId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }


        public Cart Cart { get; set; }
    }
}
