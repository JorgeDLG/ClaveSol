using Microsoft.AspNetCore.Identity;

namespace ClaveSol.Security
{
    public class appIdentityRole : IdentityRole
    {
       public string Description {get; set;}  
       //public string RoleName{get; set;}

       public appIdentityRole() : base()
       {
       }
       public appIdentityRole(string roleName) : base(roleName)
       {
           this.Name = roleName;
       }
    }
}