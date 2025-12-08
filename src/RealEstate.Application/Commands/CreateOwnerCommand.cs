using MediatR;
using RealEstate.SharedKernel;
using RealEstate.Contracts.DTOs;

namespace RealEstate.Application.Commands
{
    public record CreateOwnerCommand(
        string Name,
        string Address,
        string? Base64Photo,
        DateTime Birthday
    ) : IRequest<Result<OwnerDto>>;
}