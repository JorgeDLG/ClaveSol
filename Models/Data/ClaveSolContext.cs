using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClaveSol.Models;

    public class ClaveSolContext : DbContext
    {
        public ClaveSolContext (DbContextOptions<ClaveSolContext> options)
            : base(options)
        {
        }

        public DbSet<ClaveSol.Models.User> User { get; set; }
    }
