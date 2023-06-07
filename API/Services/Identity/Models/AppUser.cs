using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Services.Identity.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<UserAddress> UserAddresses { get; set; }
        public CurrentUsersAddress CurrentUsersAddress { get; set; }
    }
}
