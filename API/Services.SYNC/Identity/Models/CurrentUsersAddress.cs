namespace Services.Identity.Models
{
    public class CurrentUsersAddress
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }


        public AppUser AppUser { get; set; }
        public Address Address { get; set; }


    }
}
