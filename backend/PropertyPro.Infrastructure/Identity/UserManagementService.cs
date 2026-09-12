using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Users.DTOs;

namespace PropertyPro.Infrastructure.Identity;

public class UserManagementService : IUserManagementService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserManagementService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IReadOnlyList<UserManagementDto>> GetUsersAsync(
        CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .AsNoTracking()
            .OrderBy(x => x.Email)
            .ToListAsync(cancellationToken);

        var result = new List<UserManagementDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(
                new UserManagementDto(
                    user.Id,
                    user.Email ?? string.Empty,
                    user.FirstName,
                    user.LastName,
                    user.EmailConfirmed,
                    user.LockoutEnabled,
                    user.LockoutEnd.HasValue &&
                    user.LockoutEnd.Value > DateTimeOffset.UtcNow,
                    roles.ToList()));
        }

        return result;
    }

    public async Task<UserManagementDto?> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserManagementDto(
            user.Id,
            user.Email ?? string.Empty,
            user.FirstName,
            user.LastName,
            user.EmailConfirmed,
            user.LockoutEnabled,
            user.LockoutEnd.HasValue &&
            user.LockoutEnd.Value > DateTimeOffset.UtcNow,
            roles.ToList());
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return [];

        var roles = await _userManager.GetRolesAsync(user);

        return roles.ToArray();
    }

    public async Task AssignRoleAsync(
        string userId,
        string role,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new InvalidOperationException(
                "The user could not be found.");

        if (!await _roleManager.RoleExistsAsync(role))
            throw new InvalidOperationException(
                $"The role '{role}' does not exist.");

        if (await _userManager.IsInRoleAsync(user, role))
            return;

        var result = await _userManager.AddToRoleAsync(user, role);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(x => x.Description));

            throw new InvalidOperationException(
                $"The role could not be assigned: {errors}");
        }
    }

    public async Task RemoveRoleAsync(
        string userId,
        string role,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            throw new InvalidOperationException(
                "The user could not be found.");

        if (!await _roleManager.RoleExistsAsync(role))
            throw new InvalidOperationException(
                $"The role '{role}' does not exist.");

        if (!await _userManager.IsInRoleAsync(user, role))
            return;

        var result = await _userManager.RemoveFromRoleAsync(
            user,
            role);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                "; ",
                result.Errors.Select(x => x.Description));

            throw new InvalidOperationException(
                $"The role could not be removed: {errors}");
        }
    }
}