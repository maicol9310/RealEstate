using MediatR;
using RealEstate.Application.Shared;

namespace RealEstate.Application.Commands
{
    public record ChangePriceCommand(Guid PropertyId, decimal NewPrice) : IRequest<Result>;
}