using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PizzaWebsite.Controllers
{
    /// <summary>
    /// Controller that blocks unauthorized website users and forces them to login.
    /// </summary>
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPizzaWebsiteRepository _pizzaRepository;

        public CartController(ILogger<CartController> logger,
            UserManager<IdentityUser> userManager,
            IPizzaWebsiteRepository pizzaRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _pizzaRepository = pizzaRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Checkout")]
        public IActionResult Checkout()
        {
            // Checkout can only be accessed if the user has items in their cart.
            if (_pizzaRepository.GetCurrentCartItems().Count <= 0)
            {
                // Assisted by https://stackoverflow.com/questions/10785245/redirect-to-action-in-another-controller
                return RedirectToAction("Index", "Menu", new { area = "" });
            }

            return View();
        }

        [HttpGet("Items")]
        public IActionResult Items()
        {
            var cartItems = _pizzaRepository.GetCurrentCartItems();

            decimal total = 0;

            foreach (var cartItem in cartItems)
            {
                total += cartItem.UnitPrice * cartItem.Quantity;
            }

            CartViewModel cartViewModel = new CartViewModel()
            {
                CartItems = cartItems,
                Total = total
            };

            return View("Items", cartViewModel);
        }

        [HttpPost()]
        public IActionResult Add(MenuItemViewModel menuItemViewModel)
        {
            int portionId = _pizzaRepository.GetPortionIdByLabel(menuItemViewModel.ChosenProductPortion);

            CartItem cartItem = _pizzaRepository.GetCurrentCartItemByPortionIdAndProductId(menuItemViewModel.ChosenProductId, portionId);

            // If the selected product of the selected portion was not added yet to the cart
            if (cartItem == null)
            {
                // Add a new CartItem to the current Order
                _pizzaRepository.AddCurrentCartItemToDatabase(menuItemViewModel.ChosenProductId, portionId, menuItemViewModel.ChosenProductQuantity);
            }
            // If the selected product of the selected portion was already added to the cart, then increase its quantity accordingly
            else
            {
                cartItem.Quantity += menuItemViewModel.ChosenProductQuantity;
            }

            switch (cartItem.Product.Category)
            {
                case ProductCategory.Pizza:
                    return RedirectToAction("Pizzas", "Menu", new { area = "" });
                case ProductCategory.Burger:
                    return RedirectToAction("Burgers", "Menu", new { area = "" });
                case ProductCategory.Drink:
                    return RedirectToAction("Drinks", "Menu", new { area = "" });
                case ProductCategory.Side:
                    return RedirectToAction("Sides", "Menu", new { area = "" });
                default:
                    return RedirectToAction("Index", "Menu", new { area = "" });
            }
        }

        public IActionResult Delete(int productId, int portionId)
        {
            CartItem cartItem = _pizzaRepository.GetCurrentCartItemByPortionIdAndProductId(productId, portionId);

            if (cartItem == null)
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "There is no such item in the cart to remove."
                });
            }

            _pizzaRepository.GetCurrentCartItems().Remove(cartItem);

            return RedirectToAction("Items");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
