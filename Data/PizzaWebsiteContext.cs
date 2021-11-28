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
        public DbSet<ProductPortion> ProductPortions { get; set; }
        public DbSet<Portion> Portions { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CartItem> CartItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Portions)
                .WithMany(p => p.Products)
                .UsingEntity<ProductPortion>
                (pp => pp.HasOne<Portion>().WithMany(),
                 pp => pp.HasOne<Product>().WithMany());
        }
    }
}
