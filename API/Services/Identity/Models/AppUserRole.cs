using Microsoft.AspNetCore.Identity;
using Services.Identity.Models;

namespace Identity.Models
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser AppUser { get; set; }
        public AppRole AppRole { get; set; }
    }
}
