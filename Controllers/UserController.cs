using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace PizzaWebsite.Controllers
{
    /// <summary>
    /// Controller that blocks unauthorized website users and forces them to login.
    /// </summary>
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IPizzaWebsiteRepository _pizzaRepository;

        public UserController(ILogger<UserController> logger, IPizzaWebsiteRepository pizzaRepository)
        {
            _logger = logger;
            _pizzaRepository = pizzaRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Cart")]
        public IActionResult Cart()
        {
            // TODO: add IdentityRepository to get user id

            // Below code is for testing
            var products = _pizzaRepository.GetAllProducts();

            List<CartItem> cartItems = new List<CartItem>();

            decimal total = 0;
            foreach (var product in products)
            {
                CartItem cartItem = new CartItem()
                {
                    UserId = 0,
                    ProductId = product.Id,
                    Product = product,
                    Quantity = 3
                };

                total += cartItem.UnitPrice * cartItem.Quantity;

                cartItems.Add(cartItem);
            }

            CartViewModel cartViewModel = new CartViewModel()
            {
                CartItems = cartItems,
                Total = total
            };

            return View("Cart", cartViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
