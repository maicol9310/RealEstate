using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Queries;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class GetPropertyByIdHandler : IRequestHandler<GetPropertyByIdQuery, Result<Property>>
    {
        private readonly IPropertyRepository _repo;

        public GetPropertyByIdHandler(IPropertyRepository repo) => _repo = repo;

        public async Task<Result<Property>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var prop = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (prop == null) return Result<Property>.Failure("Property not found");
            return Result<Property>.Success(prop);
        }
    }
}