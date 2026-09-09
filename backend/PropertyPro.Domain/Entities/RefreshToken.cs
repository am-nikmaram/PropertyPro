using PropertyPro.Domain.Common;

namespace PropertyPro.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = null!;


public string UserId { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? ReplacedByToken { get; set; }

    public bool IsRevoked => RevokedAt.HasValue;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsActive => !IsRevoked && !IsExpired;

}
