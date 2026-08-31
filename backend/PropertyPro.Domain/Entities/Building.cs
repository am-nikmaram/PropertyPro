using PropertyPro.Domain.Common;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Domain.Entities;

public class Building : BaseEntity
{
    public int FacilityId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public int? YearBuilt { get; set; }

    public decimal? TotalArea { get; set; }

    public Facility Facility { get; set; } = null!;

    public ICollection<Floor> Floors { get; set; }
        = new List<Floor>();
}
