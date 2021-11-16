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

    public DbSet<Contact> Contacts { get; set; }
}
}
