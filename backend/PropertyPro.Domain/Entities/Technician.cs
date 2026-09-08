using PropertyPro.Domain.Common;

namespace PropertyPro.Domain.Entities;

public class Technician : BaseEntity
{
    public string EmployeeNumber { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Specialization { get; set; }

    public string? Notes { get; set; }

    public ICollection<WorkOrder> WorkOrders { get; set; }
        = new List<WorkOrder>();
}