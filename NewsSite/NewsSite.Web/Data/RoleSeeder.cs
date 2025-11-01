using Microsoft.AspNetCore.Identity;

namespace NewsSite.Web.Data
    {
    public static class RoleSeeder
        {
        public static async Task SeedRolesAndAdmin(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            {
            // создаём роль Admin если нет
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            // создаём первого админа
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
                {
                adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
