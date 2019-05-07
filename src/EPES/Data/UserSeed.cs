using EPES.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPES.Data
{
    public class UserSeed
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager)
        {
            if ((await userManager.FindByNameAsync("admin")) == null)
            {
                var admin = new ApplicationUser { UserName = "admin", Email = "admin@epes.rd.go.th" };

                await userManager.CreateAsync(admin, "P@ssw0rd");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
