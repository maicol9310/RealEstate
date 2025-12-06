using MediatR;
using RealEstate.Application.Shared;
using RealEstate.Contracts.DTOs;

namespace RealEstate.Application.Features.Owners.Commands.CreateOwner
{
    public record CreateOwnerCommand(
        string Name,
        string Address,
        string? Base64Photo,
        DateTime Birthday
    ) : IRequest<Result<OwnerDto>>;
}