using Microsoft.Extensions.DependencyInjection;
using PropertyPro.Application.Common.Security;

namespace PropertyPro.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddPropertyProAuthorization(
    this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
            AuthorizationPolicies.AdminOnly,
            policy =>
            {
                policy.RequireRole(
    IdentityRoles.Admin);
            });


        options.AddPolicy(
            AuthorizationPolicies.FacilityManagement,
            policy =>
            {
                policy.RequireRole(
                    IdentityRoles.Admin,
                    IdentityRoles.FacilityManager);
            });

            options.AddPolicy(
                AuthorizationPolicies.MaintenanceManagement,
                policy =>
                {
                    policy.RequireRole(
                        IdentityRoles.Admin,
                        IdentityRoles.MaintenanceManager);
                });

            options.AddPolicy(
                AuthorizationPolicies.MaintenanceExecution,
                policy =>
                {
                    policy.RequireRole(
                        IdentityRoles.Admin,
                        IdentityRoles.MaintenanceManager,
                        IdentityRoles.Technician);
                });

            options.AddPolicy(
                AuthorizationPolicies.ReadOnly,
                policy =>
                {
                    policy.RequireRole(
                        IdentityRoles.Admin,
                        IdentityRoles.FacilityManager,
                        IdentityRoles.MaintenanceManager,
                        IdentityRoles.Technician,
                        IdentityRoles.Viewer);
                });
        });

        return services;
    }

}
