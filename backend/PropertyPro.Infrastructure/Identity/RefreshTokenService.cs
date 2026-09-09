using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;
using PropertyPro.Infrastructure.Persistence;

namespace PropertyPro.Infrastructure.Identity;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ApplicationDbContext _context;
    private readonly RefreshTokenOptions _options;


public RefreshTokenService(
    ApplicationDbContext context,
    IOptions<RefreshTokenOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    public async Task<string> CreateAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        var token = GenerateToken();

        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(
                _options.ExpirationDays),
            
        };

        _context.RefreshTokens.Add(refreshToken);

        await _context.SaveChangesAsync(cancellationToken);

        return token;
    }

    public async Task<bool> IsActiveAsync(
        string token,
        CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .AnyAsync(
                x => x.Token == token &&
                     x.RevokedAt == null &&
                     x.ExpiresAt > DateTime.UtcNow,
                cancellationToken);
    }

    public async Task<string?> GetUserIdAsync(
        string token,
        CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .AsNoTracking()
            .Where(x =>
                x.Token == token &&
                x.RevokedAt == null &&
                x.ExpiresAt > DateTime.UtcNow)
            .Select(x => x.UserId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task RevokeAsync(
        string token,
        string? replacedByToken,
        CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(
                x => x.Token == token,
                cancellationToken);

        if (refreshToken is null)
            return;

        if (refreshToken.RevokedAt.HasValue)
            return;

        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.ReplacedByToken = replacedByToken;

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }


}
