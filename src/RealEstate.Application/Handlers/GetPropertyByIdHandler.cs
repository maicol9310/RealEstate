using MediatR;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Queries;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class GetPropertyByIdHandler : IRequestHandler<GetPropertyByIdQuery, Result<Property>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetPropertyByIdHandler> _logger;

        public GetPropertyByIdHandler(IUnitOfWork uow, ILogger<GetPropertyByIdHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result<Property>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Property in processing");
            var prop = await _uow.Properties.GetByIdAsync(request.Id, cancellationToken);
            if (prop == null) 
            {
                _logger.LogWarning("Uncreated image");
                return Result<Property>.Failure("Property not found"); 
            }
            return Result<Property>.Success(prop);
        }
    }
}