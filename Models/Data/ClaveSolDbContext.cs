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

        public DbSet<User> User { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<LineOrder> LineOrder { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<Instrument> Instrument { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<List> List { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<Operator> Operator { get; set; }
        public DbSet<Tiket> Tiket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // For N-N Join tables
            modelBuilder.Entity<List_Instrument>()
            .HasKey(t => new { t.ListId, t.InstrumentId });

            modelBuilder.Entity<List_Instrument>()
                .HasOne(pt => pt.List)
                .WithMany(p => p.List_Instruments)
                .HasForeignKey(pt => pt.ListId);

            modelBuilder.Entity<List_Instrument>()
                .HasOne(pt => pt.Instrument)
                .WithMany(t => t.List_Instruments)
                .HasForeignKey(pt => pt.InstrumentId);
        }
    }
}
