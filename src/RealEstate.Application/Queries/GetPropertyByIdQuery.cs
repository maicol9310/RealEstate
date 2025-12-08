using MediatR;
using RealEstate.SharedKernel;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Queries
{
    public record GetPropertyByIdQuery(Guid Id) : IRequest<Result<Property>>;
}