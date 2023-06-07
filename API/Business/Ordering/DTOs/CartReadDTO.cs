namespace Business.Ordering.DTOs
{
    public class CartReadDTO
    {
        public Guid CartId { get; set; }
        public int UserId { get; set; }
        public double Total { get; set; }
        public IEnumerable<CartItemReadDTO> CartItems { get; set; }
    }
}
