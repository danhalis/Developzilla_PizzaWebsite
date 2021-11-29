using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaWebsite.Data.Repositories
{
    public interface IPizzaWebsiteRepository
    {
        List<UserData> GetAllUserDatas();
        UserData GetUserDataByUserId(string userId);
        void Update(UserData userData);

        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(ProductCategory productCategory);
        
        bool SaveAll();
    }

    public class PizzaWebsiteRepository : IPizzaWebsiteRepository
    {
        private readonly PizzaWebsiteDbContext _context;
        private readonly ILogger<PizzaWebsiteRepository> _logger;

        public PizzaWebsiteRepository(ILogger<PizzaWebsiteRepository> logger, PizzaWebsiteDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<UserData> GetAllUserDatas()
        {
            try
            {
                _logger.LogInformation("Getting all user data ...");

                return _context.UserDatas.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all user data: {e}");

                return null;
            }
        }

        public UserData GetUserDataByUserId(string userId)
        {
            try
            {
                _logger.LogInformation("Getting user data by user id ...");

                return _context.UserDatas.FirstOrDefault(ud => ud.UserId == userId);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get user data by user id: {e}");

                return null;
            }
        }

        public void Update(UserData userData)
        {
            try
            {
                _logger.LogInformation("Getting user data by user id ...");

                _context.Update(userData);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get user data by user id: {e}");
            }
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

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
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
                SortPortionsAndPrices(product);
            }
        }

        private void SortPortionsAndPrices(Product product)
        {
            // Bubble sort assisted by http://anh.cs.luc.edu/170/notes/CSharpHtml/sorting.html
            int i;
            for (int j = product.Prices.Count - 1; j > 0; j--)
            {
                for (i = 0; i < j; i++)
                {
                    if (product.Prices[i] > product.Prices[i + 1])
                        SwapPortionAndPriceOfProduct(product, i, i + 1);
                }
            }
        }

        private void SwapPortionAndPriceOfProduct(Product product, int firstIndex, int secondIndex)
        {
            decimal swappedPrice = product.Prices[firstIndex];
            Portion swappedPortion = product.Portions[firstIndex];

            product.Prices[firstIndex] = product.Prices[secondIndex];
            product.Portions[firstIndex] = product.Portions[secondIndex];

            product.Prices[secondIndex] = swappedPrice;
            product.Portions[secondIndex] = swappedPortion;
        }
    }
}
