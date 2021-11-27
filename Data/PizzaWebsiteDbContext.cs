using Microsoft.EntityFrameworkCore;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data
{
    public class PizzaWebsiteDbContext : DbContext
    {
        public PizzaWebsiteDbContext(DbContextOptions<PizzaWebsiteDbContext> options) : base(options)
        {
        }

        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("money");
        }

        public DbSet<CartItem> CartItem { get; set; }
    }
}
