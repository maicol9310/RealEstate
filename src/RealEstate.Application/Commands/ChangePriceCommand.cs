using MediatR;
using RealEstate.SharedKernel;

namespace RealEstate.Application.Commands
{
    public record ChangePriceCommand(Guid PropertyId, decimal NewPrice) : IRequest<Result>;
}