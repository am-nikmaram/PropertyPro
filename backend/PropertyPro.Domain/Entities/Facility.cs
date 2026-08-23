using PropertyPro.Domain.Common;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Domain.Entities;

public class Facility : BaseEntity
{
    public int OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public Organization Organization { get; set; } = null!;

    public ICollection<Building> Buildings { get; set; }
        = new List<Building>();
}