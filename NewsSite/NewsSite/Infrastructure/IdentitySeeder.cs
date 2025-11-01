using Microsoft.AspNetCore.Identity;

namespace NewsSite.Web.Infrastructure
    {
    public static class IdentitySeeder
        {
        public static async Task SeedAsync(IServiceProvider sp)
            {
            using var scope = sp.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            var email = "admin@example.com";
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                {
                user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, "Admin#12345");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }
        }
    }
