﻿namespace CarRentalSystem.Identity.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using CarRentalSystem.Common;
    using Common.Services;
    using Microsoft.AspNetCore.Identity;
    using Models;

    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityDataSeeder(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void SeedData()
        {
            if (this.roleManager.Roles.Any())
            {
                return;
            }

            Task
                .Run(async () =>
                {
                    var adminRole = new IdentityRole(Constants.AdministratorRoleName);

                    await this.roleManager.CreateAsync(adminRole);

                    var adminUser = new User
                    {
                        UserName = "admin@crs.com",
                        Email = "admin@crs.com",
                        SecurityStamp = "RandomSecurityStamp"
                    };

                    await this.userManager.CreateAsync(adminUser, "adminpass12");

                    await this.userManager.AddToRoleAsync(adminUser, Constants.AdministratorRoleName);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}