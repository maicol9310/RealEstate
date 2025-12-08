using MediatR;
using RealEstate.Application.Shared;
using RealEstate.Contracts.DTOs;

namespace RealEstate.Application.Commands
{
    public record  RegisterSaleCommand(DateTime dateSale, string name, decimal value, decimal tax, Guid idProperty) : IRequest<Result<PropertyTraceDto>>;
}
