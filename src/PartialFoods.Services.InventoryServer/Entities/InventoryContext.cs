using Microsoft.EntityFrameworkCore;

namespace PartialFoods.Services.InventoryServer.Entities
{
    public class InventoryContext : DbContext
    {
        private string connStr;

        public InventoryContext(string connectionString) : base()
        {
            connStr = connectionString;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductActivity> Activities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connStr);
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