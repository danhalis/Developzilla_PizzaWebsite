﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using PizzaWebsite.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Data.Seeder
{
    public class UserIdentityDataSeeder
    {
        public enum Roles
        {
            Customer
        }

        private readonly IWebHostEnvironment _host;
        private readonly UserIdentityDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;


        public UserIdentityDataSeeder(IWebHostEnvironment host,
                                      UserIdentityDbContext context,
                                      RoleManager<IdentityRole> roleManager,
                                      UserManager<IdentityUser> userManager)
        {
            _host = host;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        private async Task SeedRolesAsync()
        {
            var rolesFile = Path.Combine(_host.ContentRootPath, "Data/SampleData/roles.json");

            var json = File.ReadAllText(rolesFile);

            var roles = JsonConvert.DeserializeObject<IEnumerable<IdentityRole>>(json);

            foreach (IdentityRole role in roles)
            {
                if (_context.Roles.Any(r => r.Name == role.Name))
                    continue;

                await _roleManager.CreateAsync(role);
            }
        }

        private async Task SeedUserAsync(Roles role,
                                        IdentityUser user)
        {         
            try
            {
                // create the new user with password
                IdentityResult result = await _userManager.CreateAsync(user, role + "@123");
                
                if (!result.Succeeded)
                {
                    var exceptionText = result.Errors.Aggregate("Could not create user - Identity Exception: \n\r\n\r", (current, error) => current + (" - " + error + "\n\r"));
                    throw new Exception(exceptionText);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            // add user to specified role
            switch (role)
            {
                case Roles.Customer:
                    await _userManager.AddToRoleAsync(user, Roles.Customer.ToString());
                    break;
            }
        }

        private async Task SeedUsersAsync()
        {
            var usersFile = Path.Combine(_host.ContentRootPath, "Data/SampleData/users.json");
            
            var json = File.ReadAllText(usersFile);

            var users = JsonConvert.DeserializeObject<IEnumerable<IdentityUser>>(json);
            
            foreach (IdentityUser user in users)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                    continue;

                await SeedUserAsync(Roles.Customer, user);
            }
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            await SeedRolesAsync();
            await SeedUsersAsync();
        }
    }
}