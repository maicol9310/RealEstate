using MediatR;
using RealEstate.Application.Shared;
using RealEstate.Contracts.DTOs;

namespace RealEstate.Application.Commands
{
    public record CreatePropertyImageCommand(Guid PropertyId, string ImageBase64) : IRequest<Result<PropertyImageDto>>;
}