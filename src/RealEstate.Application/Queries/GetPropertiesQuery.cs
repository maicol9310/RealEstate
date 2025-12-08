using MediatR;
using RealEstate.SharedKernel;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Queries
{
    public record GetPropertiesQuery(decimal? MinPrice, decimal? MaxPrice, Guid? OwnerId, int? Year)
        : IRequest<Result<IEnumerable<Property>>>;
}