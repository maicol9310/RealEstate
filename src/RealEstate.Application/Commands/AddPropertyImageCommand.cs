using MediatR;
using RealEstate.Application.Shared;

namespace RealEstate.Application.Commands
{
    public record AddPropertyImageCommand(Guid PropertyId, string ImageBase64) : IRequest<Result>;
}