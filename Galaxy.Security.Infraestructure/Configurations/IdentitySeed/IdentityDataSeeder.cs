using Galaxy.Security.Infraestructure.Configurations.IdentityEntities;
using Galaxy.Security.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Galaxy.Security.Infraestructure.Configurations.IdentitySeed
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider service)
        {

            var userManager = service.GetRequiredService<UserManager<UserExtension>>();
            var rolManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            //Seed Roles
            var admin = new IdentityRole(RolesConstants.AdminRole);
            var customer = new IdentityRole(RolesConstants.CustomerRole);
            var manager = new IdentityRole(RolesConstants.ManagerRole);

            if (!await rolManager.RoleExistsAsync(RolesConstants.AdminRole)) await rolManager.CreateAsync(admin);
            if (!await rolManager.RoleExistsAsync(RolesConstants.CustomerRole)) await rolManager.CreateAsync(customer);
            if (!await rolManager.RoleExistsAsync(RolesConstants.ManagerRole)) await rolManager.CreateAsync(manager);

            //Seed default user admin
            var adminUser = new UserExtension
            {
                FullName = "Juan Carlos De La Cruz",
                UserName = "admin",
                Email = "developerdelacruz96@gmail.com",
                EmailConfirmed = true,
                UserId = new("4fa2b6f5-1c3b-4e2e-8f0a-2c3b5f6e7d8a")
            };

            var result = await userManager.CreateAsync(adminUser, "Password2025");
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, RolesConstants.AdminRole);
            }

        }
    }
}
