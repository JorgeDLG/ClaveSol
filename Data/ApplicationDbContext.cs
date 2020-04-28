using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Security;

namespace ClaveSol.Data
{
    public class ApplicationDbContext : IdentityDbContext
    <AppIdentityUser,AppIdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        //public DbSet<AspNetUsers> User {get; set;}
    }
}
