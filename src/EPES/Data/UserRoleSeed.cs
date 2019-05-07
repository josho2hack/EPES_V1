using EPES.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Data
{
    public class UserRoleSeed
    {
        public static async Task Seed(RoleManager<IdentityRole> roleManager)
        {
            if ((await roleManager.FindByNameAsync("Admin")) == null )
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            if ((await roleManager.FindByNameAsync("Manager")) == null)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Manager" });
            }

            if ((await roleManager.FindByNameAsync("User")) == null)
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "User" });
            }
        }
    }
}
