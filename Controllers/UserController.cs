using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPizzaWebsiteRepository _pizzaRepository;

        public UserController(ILogger<UserController> logger,
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

        [HttpGet("Cart")]
        public IActionResult Cart()
        {
            var userId = _userManager.GetUserId(User);

            var cartItems = _pizzaRepository.GetCartItemsByUserId(userId);

            decimal total = 0;

            foreach (var cartItem in cartItems)
            {
                total += cartItem.ProductPortion.UnitPrice * cartItem.Quantity;
            }

            CartViewModel cartViewModel = new CartViewModel()
            {
                CartItems = cartItems,
                Total = total
            };

            return View("Cart", cartViewModel);
        }

        [HttpPost()]
        public IActionResult AddToCart(MenuItemViewModel menuItemViewModel)
        {
            int portionId = _pizzaRepository.GetPortionIdByName(menuItemViewModel.ChosenProductPortion);

            CartItem cartItem = _pizzaRepository.GetCartItemByProductIdAndUserIdAndProductIdAndPortionId(
                _userManager.GetUserId(User),
                menuItemViewModel.ChosenProductId,
                portionId);

            // if the selected product of the selected portion was not added yet to the cart
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    UserId = _userManager.GetUserId(User),
                    ProductId = menuItemViewModel.ChosenProductId,
                    PortionId = portionId,
                    Quantity = 1
                };

                _pizzaRepository.Add(cartItem);

                // save changes
                if (!_pizzaRepository.SaveAll())
                {
                    // redirect to an error page
                    return RedirectToAction("Error", "Home", new ErrorViewModel
                    {
                        Message = "Failed to add item to the cart."
                    });
                }
            }
            // if the selected product of the selected portion was already added to the cart
            else
            {

            }

            return RedirectToAction("Menu", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
