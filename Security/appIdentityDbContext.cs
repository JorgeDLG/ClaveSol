using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ClaveSol.Security;

public class appIdentityDbContext:IdentityDbContext
    <appIdentityUser,appIdentityRole, string> //string = TKey PK for users & roles
{
    public appIdentityDbContext(DbContextOptions<appIdentityDbContext> options) :
    base(options)
    {
    }
}
