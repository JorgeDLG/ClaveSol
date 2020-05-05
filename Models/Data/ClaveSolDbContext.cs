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
        public DbSet<Order> Order { get; set; }
        public DbSet<LineOrder> LineOrder { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<Instrument> Instrument { get; set; }
        public DbSet<ClaveSol.Models.Comment> Comment { get; set; }
        public DbSet<ClaveSol.Models.List> List { get; set; }
        public DbSet<ClaveSol.Models.Chat> Chat { get; set; }
        public DbSet<ClaveSol.Models.Operator> Operator { get; set; }
        public DbSet<ClaveSol.Models.Tiket> Tiket { get; set; }
    }
}
