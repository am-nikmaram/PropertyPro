using MediatR;

namespace PropertyPro.Application.Features.Maintenance.Commands.DeleteMaintenance;

public record DeleteMaintenanceCommand(int Id) : IRequest;