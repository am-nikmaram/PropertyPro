using PropertyPro.Domain.Common;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Domain.Entities;

public class Floor : BaseEntity
{
    public int BuildingId { get; set; }

    public string Name { get; set; } = null!;

    public int FloorNumber { get; set; }

    public string? Description { get; set; }
    public decimal? TotalArea { get; set; }

    public Building Building { get; set; } = null!;

    public ICollection<Room> Rooms { get; set; }
        = new List<Room>();
}