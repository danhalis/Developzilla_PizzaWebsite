using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using PizzaWebsite.Data;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Models.Seeder
{
    public class PizzaWebsiteDataSeeder
    {
        private readonly IWebHostEnvironment _host;
        private readonly PizzaWebsiteContext _context;

        public PizzaWebsiteDataSeeder(IWebHostEnvironment host, PizzaWebsiteContext context)
        {
            _host = host;
            _context = context;
        }

        public void Seed()
        {
            // ensure that the database exists
            _context.Database.EnsureCreated();

            // if there are no categories
            if (!_context.ProductCategories.Any())
            {
                // create sample category data

                // ContentRootPath is refering to folders not related to wwwroot
                var productCategoriesFile = Path.Combine(_host.ContentRootPath, "Data/SampleData/product-categories.json");

                var json = File.ReadAllText(productCategoriesFile);
                
                // deserialize json file into a category list
                var categories = JsonConvert.DeserializeObject<IEnumerable<ProductCategory>>(json);

                // add the product category list to the database
                _context.ProductCategories.AddRange(categories);
            }

            if (!_context.Products.Any())
            {
                // create sample product data

                // ContentRootPath is refering to folders not related to wwwroot
                var productsFile = Path.Combine(_host.ContentRootPath, "Data/SampleData/products.json");

                var json = File.ReadAllText(productsFile);

                // deserialize json file into a category list
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

                // add the product list to the database
                _context.Products.AddRange(products);
            }

            _context.SaveChanges();
        }
    }
}
