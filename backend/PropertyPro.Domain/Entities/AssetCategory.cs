using PropertyPro.Domain.Common;

namespace PropertyPro.Domain.Entities;

public class AssetCategory : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<Asset> Assets { get; set; }
        = new List<Asset>();
}