namespace PropertyPro.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }

    string? Email { get; }

    string? Name { get; }

    bool IsAuthenticated { get; }

    bool IsInRole(string role);
}