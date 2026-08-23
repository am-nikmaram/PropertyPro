
using PropertyPro.Domain.Common;

namespace PropertyPro.Domain.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<Facility> Facilities { get; set; }
        = new List<Facility>();
}