using Microsoft.AspNetCore.Identity;

using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? OrganizationId { get; set; }
    public Organization? Organization { get; set; }
}
