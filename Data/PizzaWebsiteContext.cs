using Microsoft.EntityFrameworkCore;
using PizzaWebsite.Data.Entities;

namespace PizzaWebsite.Data
{
    public class PizzaWebsiteContext : DbContext
    {
        public PizzaWebsiteContext(DbContextOptions<PizzaWebsiteContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSizePrice> SizePrices { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("money");*/
        }

        public DbSet<CartItem> CartItem { get; set; }
    }
}
