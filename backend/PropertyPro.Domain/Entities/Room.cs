using PropertyPro.Domain.Common;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Domain.Entities;

public class Room : BaseEntity
{
    public int FloorId { get; set; }
    public int RoomTypeId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    
    public decimal? Area { get; set; }

    public int? Capacity { get; set; }

    public Floor Floor { get; set; } = null!;
    public RoomType RoomType { get; set; } = null!;

    public ICollection<Asset> Assets { get; set; }
        = new List<Asset>();
}