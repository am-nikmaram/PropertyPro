using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropertyPro.Application.Common.Security;
using PropertyPro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace PropertyPro.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(
        IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<
                RoleManager<IdentityRole>>();

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

    public static async Task SeedDevelopmentAdminAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        var userManager =
            serviceProvider.GetRequiredService<
                UserManager<ApplicationUser>>();

        var email = configuration[
            "DevelopmentAdmin:Email"];

        var password = configuration[
            "DevelopmentAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var existingUser =
            await userManager.FindByEmailAsync(email);

        if (existingUser is null)
        {
            existingUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator"
            };

            var result = await userManager.CreateAsync(
                existingUser,
                password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Failed to create development admin: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(
                existingUser,
                IdentityRoles.Admin))
        {
            var result = await userManager.AddToRoleAsync(
                existingUser,
                IdentityRoles.Admin);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(x => x.Description));

                throw new InvalidOperationException(
                    $"Failed to assign Admin role: {errors}");
            }
        }
    }

    public static async Task AssignDevelopmentAdminOrganizationAsync(
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        var email = configuration["DevelopmentAdmin:Email"];
        if (string.IsNullOrWhiteSpace(email)) return;

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || user.OrganizationId.HasValue) return;

        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var organizationId = await context.Organizations
            .Where(x => x.Code == "PPC" && x.IsActive)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync();
        if (!organizationId.HasValue) return;

        user.OrganizationId = organizationId;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException($"Failed to assign development admin organization: {errors}");
        }
    }
}
