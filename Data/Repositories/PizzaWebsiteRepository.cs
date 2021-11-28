using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaWebsite.Data.Repositories
{
    public interface IPizzaWebsiteRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(ProductCategory productCategory);
        bool SaveAll();
    }

    public class PizzaWebsiteRepository : IPizzaWebsiteRepository
    {
        private readonly PizzaWebsiteContext _context;
        private readonly ILogger<PizzaWebsiteRepository> _logger;

        public PizzaWebsiteRepository(ILogger<PizzaWebsiteRepository> logger, PizzaWebsiteContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts was called...");
            try
            {
                List<Product> products = _context.Products
                    .OrderBy(p => p.Id)
                    .ToList();

                FillProductListFields(products);
                return products;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get all products: {exception}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(ProductCategory productCategory)
        {
            _logger.LogInformation("GetProductsByCategory was called...");
            try
            {
                List<Product> products = _context.Products
                    .Where(p => p.Category == productCategory)
                    .OrderBy(p => p.Id)
                    .ToList();

                FillProductListFields(products);
                return products;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get products by category: {exception}");
                return null;
            }
        }

        private void FillProductListFields(List<Product> products)
        {
            foreach (Product product in products)
            {
                List<ProductPortion> productPortions = _context.ProductPortions
                    .Where(pp => pp.ProductId == product.Id)
                    .OrderBy(pp => pp.Id)
                    .ToList();

                foreach (ProductPortion productPortion in productPortions)
                {
                    Portion portion = _context.Portions
                        .Where(p => p.Id == productPortion.PortionId)
                        .First();

                    product.Portions.Add(portion);
                    product.Prices.Add(productPortion.UnitPrice);
                }
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
