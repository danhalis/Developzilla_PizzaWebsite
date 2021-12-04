using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace PizzaWebsite.Data.Repositories
{
    public interface IPizzaWebsiteRepository
    {
        #region Order
        /// <summary>
        /// Gets the current <see cref="Order"/> of this <see cref="PizzaWebsiteRepository"/>.
        /// </summary>
        /// <returns>The current <see cref="Order"/> of this <see cref="PizzaWebsiteRepository"/>.</returns>
        Order GetCurrentOrder();

        /// <summary>
        /// Gets the <see cref="List{CartItem}"/> stored in the current <see cref="Order"/> object's <see cref="Order.CartItems"/>.
        /// </summary>
        /// <returns>The <see cref="List{CartItem}"/> stored in the current <see cref="Order"/>'s <see cref="Order.CartItems"/>.</returns>
        public List<CartItem> GetCurrentOrderCartItems();

        /// <summary>
        /// Ensures that the sent <see cref="CartItem"/> has its <see cref="CartItem.Product"/>, <see cref="CartItem.Portion"/> 
        /// and <see cref="CartItem.UnitPrice"/> properly set using its <see cref="CartItem.ProductId"/> and <see cref="CartItem.PortionId"/>, 
        /// then adds it to the current <see cref="Order"/> object's <see cref="Order.CartItems"/>.
        /// </summary>
        /// <param name="cartItem">The <see cref="CartItem"/> to be added to the current <see cref="Order"/> object's <see cref="Order.CartItems"/>.</param>
        public void AddCurrentOrderCartItem(CartItem cartItem);

        /// <summary>
        /// Gets the <see cref="CartItem"/> with a corresponding <see cref="CartItem.ProductId"/> and <see cref="CartItem.PortionId"/>
        /// if it exists, or <see cref="null"/> otherwise.
        /// </summary>
        /// <param name="productId">Id of the <see cref="Product"/>.</param>
        /// <param name="portionId">Id of the <see cref="Portion"/>.</param>
        /// <returns>The <see cref="CartItem"/> with a corresponding <see cref="CartItem.ProductId"/> and <see cref="CartItem.PortionId"/> if it exists, or <see cref="null"/> otherwise.</returns>
        public CartItem GetCartItemInCurrentOrderByPortionIdAndProductId(int productId, int portionId);
        #endregion

        #region User Data
        /// <summary>
        /// Retrieves a <see cref="List{T}"/> of all <see cref="UserData"/> from the database.
        /// </summary>
        /// <returns>The <see cref="List{T}"/> of all <see cref="UserData"/> from the database.</returns>
        List<UserData> GetAllUserDatas();

        /// <summary>
        /// Retrieves a <see cref="UserData"/> with the given user id from the database.
        /// </summary>
        /// <param name="userId">Id of the <see cref="IdentityUser"/>.</param>
        /// <returns>The <see cref="UserData"/> with the given user id from the database if it exists, null otherwise.</returns>
        UserData GetUserDataByUserId(string userId);

        /// <summary>
        /// Updates the given <see cref="UserData"/> in the database.
        /// </summary>
        /// <param name="userData">The <see cref="UserData"/> to update.</param>
        void Update(UserData userData);
        #endregion

        #region Product
        /// <summary>
        /// Retrieves a <see cref="List{T}"/> of all <see cref="Product"/> from the database.
        /// </summary>
        /// <returns>The <see cref="List{T}"/> of all <see cref="Product"/> from the database.</returns>
        List<Product> GetAllProducts();

        /// <summary>
        /// Retieves a <see cref="Product"/> with the given id from the database.
        /// </summary>
        /// <param name="id">Id of the <see cref="Product"/>.</param>
        /// <returns>The <see cref="Product"/> with the given id from the database if it exists, null otherwise.</returns>
        Product GetProductById(int id);

        /// <summary>
        /// Retrieves a <see cref="List{T}"/> of <see cref="Product"/> with the given <see cref="ProductCategory"/> from the database.
        /// </summary>
        /// <param name="productCategory">The category of the <see cref="Product"/>.</param>
        /// <returns>The <see cref="List{T}"/> of <see cref="Product"/> with the given <see cref="ProductCategory"/> from the database.</returns>
        List<Product> GetProductsByCategory(ProductCategory productCategory);
        #endregion

        #region Portion
        /// <summary>
        /// Retrieves a <see cref="Portion"/> with the given id from the database.
        /// </summary>
        /// <param name="id">Id of the <see cref="Portion"/>.</param>
        /// <returns>The <see cref="Portion"/> with the given id from the database if it exists, null otherwise.</returns>
        Portion GetPortionById(int id);

        /// <summary>
        /// Retrieves a <see cref="Portion"/> with the given label from the database.
        /// </summary>
        /// <param name="portionLabel">The label of the <see cref="Portion"/>.</param>
        /// <returns>The <see cref="Portion"/> with the given label from the database if it exists, null otherwise.</returns>
        int GetPortionIdByLabel(string portionLabel);
        #endregion

        #region Product & Portion
        /// <summary>
        /// Retrieves a <see cref="ProductPortion"/> with the given product id and portion id from the database.
        /// </summary>
        /// <param name="productId">Id of the <see cref="Product"/>.</param>
        /// <param name="portionId">Id of the <see cref="Portion"/>.</param>
        /// <returns>The <see cref="ProductPortion"/> with the given product id and portion id from the database if it exists, null otherwise.</returns>
        ProductPortion GetProductAndPortionById(int productId, int portionId);
        #endregion

        #region Cart Item
        /// <summary>
        /// Retrieves a <see cref="CartItem"/> with the given id from the database.
        /// </summary>
        /// <param name="id">Id of the <see cref="CartItem"/>.</param>
        /// <param name="attachNavigation">Whether to attach <see href="https://docs.microsoft.com/en-us/ef/ef6/fundamentals/relationships">navigation properties</see> to the <see cref="CartItem"/>.</param>
        /// <returns>The <see cref="CartItem"/> with the given id from the database if it exists, null otherwise.</returns>
        CartItem GetCartItemById(int id, bool attachNavigation = true);

        /// <summary>
        /// Adds the given <see cref="CartItem"/> to the database.<br/>
        /// However, no changes will be made until <see cref="IPizzaWebsiteRepository.SaveAll()"/> is called.
        /// </summary>
        /// <param name="cartItem">The <see cref="CartItem"/> to add.</param>
        void Add(CartItem cartItem);

        /// <summary>
        /// Updates the given <see cref="CartItem"/> in the database.<br/>
        /// However, no changes will be made until <see cref="IPizzaWebsiteRepository.SaveAll()"/> is called.
        /// </summary>
        /// <param name="cartItem">The <see cref="CartItem"/> to update.</param>
        void Update(CartItem cartItem);

        /// <summary>
        /// Removes the given <see cref="CartItem"/> from the database.<br/>
        /// However, no changes will be made until <see cref="IPizzaWebsiteRepository.SaveAll()"/> is called.
        /// </summary>
        /// <param name="cartItem">The <see cref="CartItem"/> to remove.</param>
        void Remove(CartItem cartItem);
        #endregion


        /// <summary>
        /// Saves all changes made by the previous CRUD operations before the call of this method.
        /// </summary>
        /// <returns>True if the saving succeeds, false otherwise.</returns>
        bool SaveAll();
    }

    public class PizzaWebsiteRepository : IPizzaWebsiteRepository
    {
        private readonly PizzaWebsiteDbContext _context;
        private readonly ILogger<PizzaWebsiteRepository> _logger;
        private static Order _currentOrder;

        static PizzaWebsiteRepository()
        {
            _currentOrder = new Order();
        }

        public PizzaWebsiteRepository(ILogger<PizzaWebsiteRepository> logger, PizzaWebsiteDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region Order
        public Order GetCurrentOrder()
        {
            return _currentOrder;
        }
        
        public List<CartItem> GetCurrentOrderCartItems()
        {
            return GetCurrentOrder().CartItems;
        }

        public void AddCurrentOrderCartItem(CartItem cartItem)
        {
            ProductPortion productPortion = GetProductAndPortionById(cartItem.ProductId, cartItem.PortionId);
            cartItem.Product = productPortion.Product;
            cartItem.Portion = productPortion.Portion;
            cartItem.UnitPrice = productPortion.UnitPrice;

            GetCurrentOrderCartItems().Add(cartItem);
        }

        public CartItem GetCartItemInCurrentOrderByPortionIdAndProductId(int productId, int portionId)
        {
            _logger.LogInformation($"Getting cart item in the current order ...");

            return GetCurrentOrder().CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.PortionId == portionId);
        }
        #endregion

        #region User Data
        public List<UserData> GetAllUserDatas()
        {
            try
            {
                _logger.LogInformation("Getting all user datas ...");

                return _context.UserDatas.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all user datas: {e}");

                return null;
            }
        }

        public UserData GetUserDataByUserId(string userId)
        {
            try
            {
                _logger.LogInformation($"Getting user data by user id {userId} ...");

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
                _logger.LogInformation($"Updating user data with id {userData.Id} ...");

                _context.Update(userData);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update user data with id {userData.Id}: {e}");
            }
        }
        #endregion

        #region Product
        public List<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Getting all products ...");

                List<Product> products = _context.Products
                    .OrderBy(p => p.Id)
                    .ToList();

                FillProductListFields(products);
                return products;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get all products: {e}");
                return null;
            }
        }

        public Product GetProductById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting product by id {id} ...");

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
            try
            {
                _logger.LogInformation($"Getting products by category {productCategory} ...");

                List<Product> products = _context.Products
                    .Where(p => p.Category == productCategory)
                    .OrderBy(p => p.Id)
                    .ToList();

                FillProductListFields(products);
                return products;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get products by category {productCategory}: {e}");
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
        #endregion

        #region Portion
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

        public int GetPortionIdByLabel(string portionLabel)
        {
            // TODO: Make portion label unique in the database
            try
            {
                _logger.LogInformation($"Getting portion id by name {portionLabel} ...");

                var portion = _context.Portions.FirstOrDefault(p => p.Label == portionLabel);

                if (portion == null)
                    return -1;

                return portion.Id;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get portion id by name {portionLabel}: {e}");

                return -1;
            }
        }
        #endregion

        #region Product & Portion
        public ProductPortion GetProductAndPortionById(int productId, int portionId)
        {
            try
            {
                _logger.LogInformation($"Getting product by id {productId} and portion id {portionId} ...");

                var productPortion = _context.ProductPortions.FirstOrDefault(pp => pp.ProductId == productId && pp.PortionId == portionId);

                if (productPortion == null) return null;

                productPortion.Product = GetProductById(productId);
                productPortion.Portion = GetPortionById(portionId);

                return productPortion;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get product by id {productId} and portion id {portionId}: {e}");

                return null;
            }
        }
        #endregion

        #region Cart Item
        public CartItem GetCartItemById(int id, bool attachReferences = true)
        {
            try
            {
                _logger.LogInformation($"Getting cart item by id {id} ...");

                var cartItem = _context.CartItems.FirstOrDefault(ci => ci.Id == id);

                if (cartItem == null) return null;

                ProductPortion productPortion = GetProductAndPortionById(cartItem.ProductId, cartItem.PortionId);

                if (attachReferences)
                {
                    // attach product obj on each corresponding cart item
                    cartItem.Product = productPortion.Product;
                    cartItem.Portion = productPortion.Portion;
                }

                cartItem.UnitPrice = productPortion.UnitPrice;

                return cartItem;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get cart item by id {id}: {e}");

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

        public void Update(CartItem cartItem)
        {
            try
            {
                _logger.LogInformation("Updating cart item ...");

                _context.CartItems.Update(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update cart item: {e}");
            }
        }

        public void Remove(CartItem cartItem)
        {
            try
            {
                _logger.LogInformation("Deleting cart item ...");

                _context.CartItems.Remove(cartItem);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete cart item: {e}");
            }
        }
        #endregion

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
