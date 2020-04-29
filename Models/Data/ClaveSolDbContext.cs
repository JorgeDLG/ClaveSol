using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Models;

namespace ClaveSol.Data
{
    public class ClaveSolDbContext : DbContext
    {
        public ClaveSolDbContext(DbContextOptions<ClaveSolDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User {get; set;}
    }
}
