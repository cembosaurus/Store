using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    public class AppRole : IdentityRole<int>
    {

        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}
