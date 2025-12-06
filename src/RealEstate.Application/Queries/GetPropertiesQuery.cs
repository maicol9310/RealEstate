using MediatR;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Queries
{
    public record GetPropertiesQuery(decimal? MinPrice, decimal? MaxPrice, Guid? OwnerId, int? Year, string? Search)
        : IRequest<Result<IEnumerable<Property>>>;
}