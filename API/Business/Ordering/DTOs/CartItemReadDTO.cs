namespace Business.Ordering.DTOs
{
    public class CartItemReadDTO
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public double SalePrice { get; set; }
        public DateTime Locked { get; set; }
    }
}
