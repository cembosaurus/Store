namespace Services.Ordering.Models
{
    public class ActiveCart
    {
        public int UserId { get; set; }
        public Guid CartId { get; set; }



        public Cart Cart { get; set; }

    }
}
