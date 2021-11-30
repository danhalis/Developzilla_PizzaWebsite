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

        List<Product> GetAllProducts();
        Product GetProductById(int id);
        List<Product> GetProductsByCategory(ProductCategory productCategory);

        Portion GetPortionById(int id);
        int GetPortionIdByName(string portionName);

        ProductPortion GetProductAndPortionById(int productId, int portionId);

        List<CartItem> GetCartItemsByUserId(string userId);
        CartItem GetCartItemByProductIdAndUserIdAndProductIdAndPortionId(string userId, int productId, int portionId);
        void Add(CartItem cartItem);

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
                _logger.LogError($"Failed to get user data by user id {userId}: {e}");

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
                _logger.LogError($"Failed to update user data: {e}");
            }
        }

        public List<Product> GetAllProducts()
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

        public Product GetProductById(int id)
        {
            _logger.LogInformation($"Getting product by id {id} ...");
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);

                FillProductListFields(product);
                return product;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get product by id {id}: {e}");

                return null;
            }
        }

        public List<Product> GetProductsByCategory(ProductCategory productCategory)
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

        private void FillProductListFields(Product product)
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

        private void FillProductListFields(List<Product> products)
        {
            foreach (Product product in products)
            {
                FillProductListFields(product);
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

        public Portion GetPortionById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting portion by id {id} ...");

                var portion = _context.Portions.FirstOrDefault(p => p.Id == id);

                return portion;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get portion by user id {id}: {e}");

                return null;
            }
        }

        public int GetPortionIdByName(string portionName)
        {
            // TODO: Make portion label unique in the database
            try
            {
                _logger.LogInformation($"Getting portion id by name {portionName} ...");

                var portion = _context.Portions.FirstOrDefault(p => p.Label == portionName);

                if (portion == null)
                    return -1;

                return portion.Id;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get portion id by name {portionName}: {e}");

                return -1;
            }
        }

        public ProductPortion GetProductAndPortionById(int productId, int portionId)
        {
            try
            {
                _logger.LogInformation($"Getting product by id {productId} with portion id {portionId} ...");

                var productPortion = _context.ProductPortions.FirstOrDefault(pp => pp.ProductId == productId && pp.PortionId == portionId);

                if (productPortion == null) return null;

                productPortion.Product = GetProductById(productId);
                productPortion.Portion = GetPortionById(portionId);

                return productPortion;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get product by id {productId} with portion id {portionId}: {e}");

                return null;
            }
        }

        public List<CartItem> GetCartItemsByUserId(string userId)
        {
            try
            {
                _logger.LogInformation($"Getting cart items by user id {userId} ...");

                var cartItems = _context.CartItems.Where(ci => ci.UserId == userId).ToList();

                // attach product obj on each corresponding cart item
                foreach (var cartItem in cartItems)
                {
                    ProductPortion productPortion = GetProductAndPortionById(cartItem.ProductId, cartItem.PortionId);

                    cartItem.ProductPortion = productPortion;
                }

                return cartItems;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get cart items by user id {userId}: {e}");

                return null;
            }
        }

        public CartItem GetCartItemByProductIdAndUserIdAndProductIdAndPortionId(string userId, int productId, int portionId)
        {
            try
            {
                _logger.LogInformation($"Getting cart items by user id {userId} ...");

                var cartItem = _context.CartItems.FirstOrDefault(ci => 
                    ci.UserId == userId && ci.ProductId == productId && ci.PortionId == portionId
                );

                if (cartItem == null) return null;

                // attach product obj on each corresponding cart item
                ProductPortion productPortion = GetProductAndPortionById(productId, portionId);

                cartItem.ProductPortion = productPortion;

                return cartItem;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get cart items by user id {userId}: {e}");

                return null;
            }
        }

        public void Add(CartItem cartItem)
        {
            try
            {
                _logger.LogInformation("Adding cart item ...");

                _context.CartItems.Add(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add cart item: {e}");
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
