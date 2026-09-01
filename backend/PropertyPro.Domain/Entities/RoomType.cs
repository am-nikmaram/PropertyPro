using PropertyPro.Domain.Common;

namespace PropertyPro.Domain.Entities;

public class RoomType : BaseEntity
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<Room> Rooms { get; set; }
        = new List<Room>();
}