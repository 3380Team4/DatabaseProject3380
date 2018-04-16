using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThemeParkApplication.Models;

namespace ThemeParkApplication.Configuration
{
    public static class UserRoleSeed
    {
        public static async Task createRoles(IServiceProvider serviceProvider, IConfiguration iconfiguration)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Manager", "Employee" };
            IdentityResult roleResult;
            
            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = new ApplicationUser
            {
                UserName = "admin@email.com",
                Email = "admin@email.com"
            };
            
            string userPassword = "P@ssw0rd";
            var user = await UserManager.FindByEmailAsync("admin@email.com");
            
            if (user == null)
            {
                var createAdminUser = await UserManager.CreateAsync(adminUser, userPassword);
                if (createAdminUser.Succeeded)
                {
                    // here we assign the new user the "Admin" role 
                    await UserManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var managerUser = new ApplicationUser
            {
                UserName = "manager@email.com",
                Email = "manager@email.com"
            };

            string managerPassword = "P@ssw0rd";
            var manager = await UserManager.FindByEmailAsync("manager@email.com");

            if (manager == null)
            {
                var createManager = await UserManager.CreateAsync(managerUser, managerPassword);
                if (createManager.Succeeded)
                {
                    // here we assign the new user the "Admin" role 
                    await UserManager.AddToRoleAsync(managerUser, "Manager");
                }
            }


            // creating a super user who could maintain the web app

        }

    }
}
