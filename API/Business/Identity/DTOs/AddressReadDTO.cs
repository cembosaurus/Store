namespace Business.Identity.DTOs
{
    public class AddressReadDTO
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
    }
}
