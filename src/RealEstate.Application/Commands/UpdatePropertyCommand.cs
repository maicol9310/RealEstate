using MediatR;
using RealEstate.Application.Shared;

namespace RealEstate.Application.Commands
{
    public record UpdatePropertyCommand(Guid PropertyId, string Name, string Address, decimal Price, string CodeInternal, int Year, Guid IdOwner)
        : IRequest<Result>;
}