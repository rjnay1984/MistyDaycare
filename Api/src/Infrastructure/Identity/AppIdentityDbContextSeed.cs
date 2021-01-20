using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MistyDaycare.ApplicationCore.Constants;

namespace MistyDaycare.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Administrators"));

            var defaultUser = new ApplicationUser { UserName = "demouser@microsoft.com", Email = "demouser@microsoft.com" };
            await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);

            string adminUserName = "admin@microsoft.com";
            var adminUser = new ApplicationUser { UserName = adminUserName, Email = adminUserName };
            await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, "Administrators");
        }
    }
}