using Microsoft.AspNetCore.Identity;

namespace ClaveSol.Security
{
    public class appIdentityUser : IdentityUser
    {
       public string FullName {get; set;} 
    }
}