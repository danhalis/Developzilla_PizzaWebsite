using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PizzaWebsite.Controllers
{
    [Authorize(Roles = "Owner, Cook, Manager, Front, Deliverer")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPizzaWebsiteRepository _pizzaRepository;

        public EmployeeController(ILogger<EmployeeController> logger,
    UserManager<IdentityUser> userManager,
    IPizzaWebsiteRepository pizzaRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _pizzaRepository = pizzaRepository;
        }

        public IActionResult Index()
        {
            IActionResult result;

            if (User.IsInRole("Owner"))
            {
                result = RedirectToAction("Owner", "Employee");
            }
            else if (User.IsInRole("Manager"))
            {
                result = RedirectToAction("Manager", "Employee");
            }
            else if (User.IsInRole("Front"))
            {
                result = RedirectToAction("Front", "Employee");
            }
            else if (User.IsInRole("Deliverer"))
            {
                result = RedirectToAction("Deliverer", "Employee");
            }
            else if (User.IsInRole("Cook"))
            {
                result = RedirectToAction("Cook", "Employee");
            }
            else
            {
                result = View();
            }

            return result;
        }

        [Authorize(Roles = "Owner")]
        public IActionResult Owner()
        {
            return View();
        }

        [Authorize(Roles = "Owner, Manager")]
        public IActionResult Manager(string searchString)
        {

            var orders = _pizzaRepository.GetAllOrdersSortByTime();
            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.CustomerFirstName.Contains(searchString)).ToList();
            }
            Dictionary<int, decimal> totalForeachOrder = new Dictionary<int, decimal>();
            decimal total = 0;


            foreach (var order in orders)
            {
                totalForeachOrder.Add(order.Id, _pizzaRepository.GetOrderTotal(order.CartId));
                total += _pizzaRepository.GetOrderTotal(order.CartId);

            }

            ManageOrderViewModel manageOrderViewModel = new ManageOrderViewModel()
            {
                Orders = orders,
                TotalForeachOrder = totalForeachOrder,
                Total = total
            };

            return View(manageOrderViewModel);
        }
        [Authorize(Roles = "Owner, Manager")]
        public IActionResult DeleteOrder(int id)
        {


            Order order = _pizzaRepository.GetOrderById(id);

            if (order == null)
            {
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "There is no such order  to remove."
                });
            }
            _pizzaRepository.Remove(order);
           
            if (!_pizzaRepository.SaveAll())
            {
                
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to remove order."
                });
            }

            return RedirectToAction("Manager");
        }

        [Authorize(Roles = "Front, Owner, Manager")]
        public IActionResult Front()
        {
            return View();
        }

        [Authorize(Roles = "Cook, Owner, Manager")]
        public IActionResult Cook()
        {
            ViewBag.Orders = _pizzaRepository.GetAllOrders();
            return View();
        }

        [Authorize(Roles = "Deliverer, Owner, Manager")]
        public IActionResult Deliverer()
        {
            ViewBag.Orders = _pizzaRepository.GetAllOrders();
            return View();
        }

        
        public IActionResult UpdateOrderStatus(int orderId, int cartId, Status newStatus, string redirectPage)
        {

            if(redirectPage == null || redirectPage.Length <= 0)
            {
                _logger.LogWarning("UpdateOrderStatus: the redirect page of is either null or has a length of 0 or less.");
            }

            Order order = _pizzaRepository.GetOrderById(orderId);

            _logger.LogDebug("UpdateOrderStatus: status is being set to " + newStatus + " on id " + orderId + " and updated in the db.");

            Status pastStatus = order.Status;
            order.Status = newStatus;

            switch (newStatus)
            {
                case Status.Preparing:
                    order.TimeAccepted = DateTime.Now;
                    break;
                case Status.Ready:
                    order.TimeCompleted = DateTime.Now;
                    break;
                case Status.Pending:
                    _logger.LogDebug("Status is Ready -> Pending");

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    order.DevilvererId = userId;
                    break;
                case Status.Completed:
                    _logger.LogDebug("Status is Pending -> Complete");
                    if (order.DevilvererId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                    {
                        order.Status = pastStatus;
                        _logger.LogWarning("UpdateOrderStatus: An complete was made by a different user than accepted it.");
                        return RedirectToAction(redirectPage);
                    }

                    break;
            }


            _pizzaRepository.Update(order);

            return RedirectToAction(redirectPage);
        }
    }
}
