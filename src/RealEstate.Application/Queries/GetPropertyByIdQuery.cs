using MediatR;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Queries
{
    public record GetPropertyByIdQuery(Guid Id) : IRequest<Result<Property>>;
}