using Microsoft.EntityFrameworkCore;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data
{
    public class PizzaWebsiteContext : DbContext
    {
        public PizzaWebsiteContext(DbContextOptions<PizzaWebsiteContext> options) : base(options)
        {
        }

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
