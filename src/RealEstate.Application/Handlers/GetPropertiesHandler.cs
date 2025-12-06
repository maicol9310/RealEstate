using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Queries;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class GetPropertiesHandler : IRequestHandler<GetPropertiesQuery, Result<IEnumerable<Property>>>
    {
        private readonly IPropertyRepository _repo;

        public GetPropertiesHandler(IPropertyRepository repo) => _repo = repo;

        public async Task<Result<IEnumerable<Property>>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            var filter = new PropertyFilter(request.MinPrice, request.MaxPrice, request.OwnerId, request.Year, request.Search);
            var list = await _repo.ListAsync(filter, cancellationToken);
            return Result<IEnumerable<Property>>.Success(list);
        }
    }
}