using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClaveSol.Data
{
    public class ClaveSolDbContext : IdentityDbContext
    {
        public ClaveSolDbContext(DbContextOptions<ClaveSolDbContext> options)
            : base(options)
        {
        }
    }
}
