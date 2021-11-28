using Microsoft.AspNetCore.Hosting;
using PizzaWebsite.Data;
using PizzaWebsite.Data.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

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

            // if there are no Products
            if (!_context.Products.Any())
            {
                // create sample product data

                // ContentRootPath is refering to folders not related to wwwroot
                var productsFile = Path.Combine(_host.ContentRootPath, "Data/SampleData/products.json");
                var json = File.ReadAllText(productsFile);

                // deserialize json file into a list of deserializedProducts
                var deserializedProducts = JsonSerializer.Deserialize<IEnumerable<DeserializedProduct>>(json);

                foreach (DeserializedProduct deserializedProduct in deserializedProducts)
                {
                    Product product = new Product()
                    {
                        Id = deserializedProduct.Id,
                        Name = deserializedProduct.Name,
                        Description = deserializedProduct.Description,
                        ImageName = deserializedProduct.ImageName
                    };

                    // Add Category
                    product.Category = deserializedProduct.Category.ToUpper() switch
                    {
                        "PIZZA" => ProductCategory.Pizza,
                        "BURGER" => ProductCategory.Burger,
                        "DRINK" => ProductCategory.Drink,
                        _ => ProductCategory.Side,
                    };

                    // Add all Sizes + Prices
                    for (int sizePriceIterator = 0; sizePriceIterator < deserializedProduct.Prices.Count; sizePriceIterator++)
                    {
                        ProductSizePrice productSizePrice = new ProductSizePrice()
                        {
                            Size = deserializedProduct.Sizes[sizePriceIterator],
                            Price = deserializedProduct.Prices[sizePriceIterator],
                            Product = product
                        };
                        _context.SizePrices.Add(productSizePrice);
                        product.SizePrices.Add(productSizePrice);
                    }

                    _context.Products.Add(product);
                }
            }

            // Commit changes to the database
            _context.SaveChanges();
        }
    }
}
