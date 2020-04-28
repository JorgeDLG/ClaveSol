using Microsoft.AspNetCore.Identity;
namespace ClaveSol.Security
{
    public class AppIdentityRole : IdentityRole
    {
        public string Description { get; set; }
        //public string RoleName{get; set;}

        public AppIdentityRole() : base()
        {
        }
        public AppIdentityRole(string roleName) : base(roleName)
        {
            this.Name = roleName;
        }
    }
}