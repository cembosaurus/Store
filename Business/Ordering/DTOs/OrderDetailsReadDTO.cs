using Business.Identity.DTOs;

namespace Business.Ordering.DTOs
{
    public class OrderDetailsReadDTO
    {
        public string? Name { get; set; }
        public AddressReadDTO? Address { get; set; }
    }
}
