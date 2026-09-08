using MediatR;

namespace PropertyPro.Application.Features.WorkOrders.Commands.DeleteWorkOrder;

public record DeleteWorkOrderCommand(int Id) : IRequest;