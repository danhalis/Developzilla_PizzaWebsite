using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PizzaWebsite.Data.Seeder
{
    public class PizzaWebsiteDataSeeder
    {
        private readonly IWebHostEnvironment _host;
        private readonly UserIdentityDbContext _identityContext;
        private readonly PizzaWebsiteDbContext _context;

        public PizzaWebsiteDataSeeder(IWebHostEnvironment host, UserIdentityDbContext identityContext, PizzaWebsiteDbContext context)
        {
            _host = host;
            _identityContext = identityContext;
            _context = context;
        }

        public void Seed()
        {
            // ensure that the database exists
            _context.Database.EnsureCreated();

            // if there are no user datas
            if (!_context.UserDatas.Any())
            {
                var userdatasFile = Path.Combine(_host.ContentRootPath, "Data/SampleData/userdatas.json");
                
                var userdatasJson = File.ReadAllText(userdatasFile);
                
                var userdatas = JsonConvert.DeserializeObject<IEnumerable<UserData>>(userdatasJson).ToList();

                _context.UserDatas.AddRange(userdatas);

                var users = _identityContext.Users.ToList();

                if (users.Count < userdatas.Count)
                {
                    throw new InvalidOperationException("Could not create user datas due to missing users.");
                }

                // attach user ids to user datas
                for (int i = 0; i < userdatas.Count; i++)
                {
                    userdatas[i].UserId = users[i].Id;
                }
            }

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
