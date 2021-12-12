using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data.Repositories;

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
        public IActionResult Manager()
        {
            return View();
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
            return View();
        }
    }
}
