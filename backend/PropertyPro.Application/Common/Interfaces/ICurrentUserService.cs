namespace PropertyPro.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }

    int? OrganizationId { get; }

    string? Email { get; }

    string? Name { get; }

    bool IsAuthenticated { get; }

    bool IsInRole(string role);
}
