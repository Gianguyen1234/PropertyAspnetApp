using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = serviceProvider.GetRequiredService<ILogger<SeedData>>();

        // Create the Admin role if it doesn't exist
        string roleName = "Admin";
        if (await roleManager.FindByNameAsync(roleName) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
            logger.LogInformation($"Role '{roleName}' created.");
        }

        // Create an admin user if it doesn't exist
        var adminEmail = "admin@gmail.com"; // Change to your desired email
        var adminPassword = "Admin@123"; // Change to your desired password

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, roleName);
                logger.LogInformation($"Admin user '{adminEmail}' created and added to role '{roleName}'.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    logger.LogError($"Error creating admin user: {error.Description}");
                }
            }
        }
    }
}
