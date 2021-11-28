using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PizzaWebsite.Controllers
{
    public class AccountController: Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public AccountController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, false);
                
                if (result.Succeeded)
                {

                    if (viewModel.IsEmployee)
                    {
                        var user = await _userManager.FindByEmailAsync(viewModel.Email);
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains("Owner")) 
                        {
                            // Redirect to the homepage of the Admin Controller
                            return RedirectToAction("Menu", "Home");
                        }
                        else if (roles.Contains("Manager"))
                        {
                            // Redirect to the homepage of the Manager Controller
                            return RedirectToAction("Menu", "Home");
                        }
                        else if (roles.Contains("Cook"))
                        {
                            // Redirect to the homepage of the Cook Controller
                            return RedirectToAction("Menu", "Home");
                        }
                        else if (roles.Contains("Deliverer"))
                        {
                            // Redirect to the homepage of the Deliverer Controller
                            return RedirectToAction("Menu", "Home");
                        }
                        else if (roles.Contains("Front"))
                        {
                            // Redirect to the homepage of the Front Controller
                            return RedirectToAction("Menu", "Home");
                        }
                        else
                        {
                            await _signInManager.SignOutAsync();
                            ModelState.AddModelError(string.Empty, "The user is not an employee please uncheck the employee box to sign into the customer login.");
                            return View(viewModel);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Login Failed. Please make sure your account data is valid.");

            }
            return View(viewModel); 
        }
    }
}
