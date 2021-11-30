using Microsoft.EntityFrameworkCore;
using PizzaWebsite.Data.Entities;

namespace PizzaWebsite.Data
{
    public class PizzaWebsiteDbContext : DbContext
    {
        public PizzaWebsiteDbContext(DbContextOptions<PizzaWebsiteDbContext> options) : base(options)
        {
        }

        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPortion> ProductPortions { get; set; }
        public DbSet<Portion> Portions { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Portions)
                .WithMany(p => p.Products)
                .UsingEntity<ProductPortion>
                (pp => pp.HasOne<Portion>().WithMany(),
                 pp => pp.HasOne<Product>().WithMany());

            modelBuilder.Entity<ProductPortion>()
                .Property(pp => pp.UnitPrice)
                .HasColumnType("money");
        }
    }
}
