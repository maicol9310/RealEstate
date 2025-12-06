using MediatR;
using RealEstate.Application.Shared;
using RealEstate.Contracts.DTOs;

namespace RealEstate.Application.Commands
{
    public record CreatePropertyCommand(string Name, string Address, decimal Price, string CodeInternal, int Year, Guid IdOwner, string? ImageBase64)
        : IRequest<Result<PropertyDto>>;
}