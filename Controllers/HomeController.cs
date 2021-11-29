﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaWebsite.Data;
using PizzaWebsite.Data.Entities;
using PizzaWebsite.Data.Repositories;
using PizzaWebsite.Models;
using PizzaWebsite.Services.GoogleMaps;
using PizzaWebsite.Services.reCAPTCHA_v2;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PizzaWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PizzaWebsiteContext _context;
        private readonly IReCaptchaVerifier _reCaptchaVerifier;
        private readonly IGeocoder _geocoder;
        private readonly IEmailSender _emailSender;
        private readonly IPizzaWebsiteRepository _pizzaWebsiteRepository;


        public HomeController(ILogger<HomeController> logger, PizzaWebsiteContext context, IPizzaWebsiteRepository pizzaWebsiteRepository, IReCaptchaVerifier reCaptchaVerifier, IGeocoder geocoder, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _pizzaWebsiteRepository = pizzaWebsiteRepository;
            _reCaptchaVerifier = reCaptchaVerifier;
            _geocoder = geocoder;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Menu")]
        public IActionResult Menu()
        {
            return View();
        }

        [HttpGet("Contact")]
        public IActionResult Contact()
        {
            // get Google Maps api key
            ViewData["GoogleMapsApiKey"] = _geocoder.GetApiKey();

            // get company location & address
            Location location = _geocoder.GetCompanyGeometryLocation();
            ViewData["CompanyAddress"] = _geocoder.GetCompanyAddress();
            ViewData["Latitude"] = location.Latitude;
            ViewData["Longitude"] = location.Longitude;

            // get reCAPTCHA site key
            ViewData["ReCaptchaSiteKey"] = _reCaptchaVerifier.GetSiteKey();

            return View();
        }

        [HttpPost("ContactSuccess")]
        public async Task<IActionResult> ContactSuccess(ContactViewModel contactForm)
        {
            string encodedReCaptchaResponse = contactForm.ReCaptchaResponse;
            bool isCaptchaValid = _reCaptchaVerifier.Validate(encodedReCaptchaResponse);

            // if the reCAPTCHA verification failed
            if (!isCaptchaValid)
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to verify reCAPTCHA response."
                });
            }

            // if the view model is not valid
            if (!ModelState.IsValid)
            {
                // redirect to an error page
                return RedirectToAction("Error", "Home", new ErrorViewModel
                {
                    Message = "Failed to send the form."
                });
            }

            // send the email
            await _emailSender.SendEmailAsync(contactForm.Email, contactForm.Topic, contactForm.Message);

            // convert the contact view model into a contact model 
            Contact contact = ConvertToContact(contactForm);

            // add the contact to the database
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            // return contact success page
            return View("ContactSuccess", contactForm);
        }

        /// <summary>
        /// Converts a ContactViewModel into a Contact model.
        /// </summary>
        /// <param name="contactViewModel">The contact view model.</param>
        /// <returns>The corresponding contact model with created timestamp.</returns>
        private static Contact ConvertToContact(ContactViewModel contactViewModel)
        {
            return new Contact()
            {
                FirstName = contactViewModel.FirstName,
                LastName = contactViewModel.LastName,
                Email = contactViewModel.Email,
                Topic = contactViewModel.Topic,
                Message = contactViewModel.Message,
                CreatedAt = DateTime.Now
            };
        }

        [HttpGet("About")]
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            
            return View();
        }

        [HttpGet("Pizzas")]
        public IActionResult Pizzas()
        {
            ViewBag.Title = "Pizza Menu";

            var pizzas = _pizzaWebsiteRepository.GetProductsByCategory(ProductCategory.Pizza);

            return View(pizzas);
        }

        [HttpGet("Drinks")]
        public IActionResult Drinks()
        {
            ViewBag.Title = "Drink Menu";

            var drinks = _pizzaWebsiteRepository.GetProductsByCategory(ProductCategory.Drink);

            return View(drinks);
        }

        [HttpGet("Burgers")]
        public IActionResult Burgers()
        {
            ViewBag.Title = "Burger Menu";

            var burgers = _pizzaWebsiteRepository.GetProductsByCategory(ProductCategory.Burger);

            return View(burgers);
        }

        [HttpGet("Sides")]
        public IActionResult Sides()
        {
            ViewBag.Title = "Side Menu";

            var sides = _pizzaWebsiteRepository.GetProductsByCategory(ProductCategory.Side);

            return View(sides);
        }

        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Job")]
        public IActionResult Job()
        {
            return View();
        }

        [HttpGet("Covid")]
        public IActionResult Covid()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
