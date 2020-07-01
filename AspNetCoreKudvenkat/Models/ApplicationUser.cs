using Microsoft.AspNetCore.Identity;

namespace AspNetCoreKudvenkat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
        
    }
}