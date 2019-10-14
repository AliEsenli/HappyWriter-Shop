using HappyWriter.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HappyWriter.Data
{
    // Seed Admin and Admin Role in Database and add Admin to Admin Role.
    public class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Using Asynchronous in Synchronous functions with Tasks
            Task<IdentityResult> roleResult;

            Task<bool> adminRole = roleManager.RoleExistsAsync("Admin");
            adminRole.Wait();

            // Create Admin Role if not exists
            if (!adminRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Admin"));
                roleResult.Wait();
            }

            if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
            {
                // Create Admin
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Id = "1",
                    Name = "adminson",
                    Vorname = "admin",
                    Wohnort = "admincity",
                    Strasse = "adminstreet",
                    Postleitzahl = 8888
                };

                IdentityResult result = userManager.CreateAsync(user, "Klapp42stuhl!").Result;
                if (result.Succeeded)
                {
                    // Asign Admin Role
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
