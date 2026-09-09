namespace PropertyPro.Application.Features.Auth.DTOs;

public record AuthResponseDto(
string UserId,
string Email,
string FirstName,
string LastName,
string Token,
string RefreshToken,
DateTime ExpiresAt,
IReadOnlyList<string> Roles);
