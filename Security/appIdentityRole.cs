using Microsoft.AspNetCore.Identity;

namespace ClaveSol.Security
{
    public class appIdentityRole : IdentityRole
    {
       public string Description {get; set;}  

       public appIdentityRole(string roleName)
       {
       }
    }
}