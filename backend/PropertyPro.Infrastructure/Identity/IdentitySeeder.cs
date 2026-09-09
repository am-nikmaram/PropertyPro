using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyPro.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(
    IServiceProvider serviceProvider)
    {
        var roleManager =
        serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


    foreach (var roleName in IdentityRoles.All)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            var result = await roleManager.CreateAsync(
                new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Failed to create role '{roleName}': {errors}");
            }
        }
    }


}
