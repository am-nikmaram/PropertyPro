using PropertyPro.Domain.Common;

namespace PropertyPro.Domain.Entities;

public class Vendor : BaseEntity
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ContactPerson { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public ICollection<WorkOrder> WorkOrders { get; set; }
        = new List<WorkOrder>();
}