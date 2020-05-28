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


            modelBuilder.Entity<Shop_Ins>()
            .HasKey(t => new { t.ShopId, t.InstrumentId });

            modelBuilder.Entity<Shop_Ins>()
                .HasOne(pt => pt.Shop)
                .WithMany(p => p.Shop_Inss)
                .HasForeignKey(pt => pt.ShopId);

            modelBuilder.Entity<Shop_Ins>()
                .HasOne(pt => pt.Instrument)
                .WithMany(t => t.Shop_Inss)
                .HasForeignKey(pt => pt.InstrumentId);


            modelBuilder.Entity<Attribut_Ins>()
            .HasKey(t => new { t.AttributId, t.InstrumentId });

            modelBuilder.Entity<Attribut_Ins>()
                .HasOne(pt => pt.Attribut)
                .WithMany(p => p.Attribut_Inss)
                .HasForeignKey(pt => pt.AttributId);

            modelBuilder.Entity<Attribut_Ins>()
                .HasOne(pt => pt.Instrument)
                .WithMany(t => t.Attribut_Inss)
                .HasForeignKey(pt => pt.InstrumentId);
        }

        public DbSet<Shop> Shop { get; set; }
        public DbSet<Attribut> Attribut { get; set; }

        //N-N DBSETS
        public DbSet<Attribut_Ins> Attribut_Ins { get; set; }
        public DbSet<List_Instrument> List_Instrument { get; set; }
        public DbSet<Shop_Ins> Shop_Ins { get; set; }
    }
}
