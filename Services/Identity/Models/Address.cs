namespace Services.Identity.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }


        public ICollection<UserAddress> UserAddresses { get; set; }
        public ICollection<CurrentUsersAddress> CurrentUsersAddresses { get; set; }

    }
}
