using PropertyPro.Domain.Common;

namespace PropertyPro.Domain.Entities;

public class Unit : BaseEntity
{
    public int FloorId { get; set; }


    public string Code { get; set; } = null!;


    public string Name { get; set; } = null!;


    public string? Description { get; set; }


    public decimal? Area { get; set; }


    public Floor Floor { get; set; } = null!;


    public ICollection<Room> Rooms { get; set; }
        = new List<Room>();
}