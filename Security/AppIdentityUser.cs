using Microsoft.AspNetCore.Identity;

namespace ClaveSol.Security
{
    public class AppIdentityUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}