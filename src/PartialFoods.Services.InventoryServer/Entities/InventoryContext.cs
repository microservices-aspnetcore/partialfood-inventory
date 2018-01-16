using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PartialFoods.Services.InventoryServer.Entities
{
    public class InventoryContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductActivity> Activities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Program.Configuration.GetConnectionString("inventory"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .HasKey(p => p.SKU);

            builder.Entity<ProductActivity>()
                .HasKey(pa => new { pa.ActivityID });
        }
    }
}