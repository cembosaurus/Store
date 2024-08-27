namespace Services.Ordering.Models
{
    public class Cart
    {
        public Guid CartId { get; set; }
        public int UserId { get; set; }        
        public double Total { get; set; }

        public Order Order { get; set; }        
        public ICollection<CartItem> CartItems { get; set; }
        public ActiveCart ActiveCart { get; set; }
    }
}
