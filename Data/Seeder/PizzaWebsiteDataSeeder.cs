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

                List<Portion> portions = new List<Portion>();

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

                    // Add all Portions + Prices
                    for (int portionPriceIterator = 0; portionPriceIterator < deserializedProduct.Prices.Count; portionPriceIterator++)
                    {
                        // Get or create a new Portion, depending on if it exists or not
                        Portion portion = portions.Where(p => p.Label.ToUpper() == deserializedProduct.Portions[portionPriceIterator].ToUpper()).FirstOrDefault();
                        if (portion == null)
                        {
                            portion = new Portion()
                            {
                                Id = 0,
                                Label = deserializedProduct.Portions[portionPriceIterator]
                            };
                            portions.Add(portion);
                        }

                        // Set up the ProductPortion that connects the Product and Portion
                        ProductPortion productPortion = new ProductPortion()
                        {
                            Id = 0,
                            Product = product,
                            Portion = portion,
                            UnitPrice = deserializedProduct.Prices[portionPriceIterator]
                        };

                        // Finalize the relationship between the Product and Portion
                        //portion.Products.Add(product);
                        //product.Portions.Add(portion);
                        _context.ProductPortions.Add(productPortion);
                    }

                    // Add the product now that it is complete
                    _context.Products.Add(product);
                }

                // Add the portions now that they are complete
                _context.Portions.AddRange(portions);
            }

            // Commit changes to the database
            _context.SaveChanges();
        }


    }
}
