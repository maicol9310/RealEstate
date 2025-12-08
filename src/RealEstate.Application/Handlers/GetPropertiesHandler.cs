using MediatR;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Queries;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class GetPropertiesHandler : IRequestHandler<GetPropertiesQuery, Result<IEnumerable<Property>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetPropertiesHandler> _logger;

        public GetPropertiesHandler(IUnitOfWork uow, ILogger<GetPropertiesHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<Property>>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("List of properties in processing");
            var filter = new PropertyFilter(request.MinPrice, request.MaxPrice, request.OwnerId, request.Year);
            var list = await _uow.Properties.ListAsync(filter, cancellationToken);
            return Result<IEnumerable<Property>>.Success(list);
        }
    }
}