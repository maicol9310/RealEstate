using MediatR;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Commands;
using RealEstate.Application.Interfaces;
using RealEstate.SharedKernel;

namespace RealEstate.Application.Handlers
{
    public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UpdatePropertyHandler> _logger;

        public UpdatePropertyHandler(IUnitOfWork uow, ILogger<UpdatePropertyHandler> logger)
        { 
            _uow = uow; 
            _logger = logger; 
        } 

        public async Task<Result> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing update request for {PropertyId}", request.PropertyId);

            var p = await _uow.Properties.GetByIdAsync(request.PropertyId, cancellationToken);

            if (p == null)
            {
                _logger.LogWarning("Property not found");
                return Result.Failure("Property not found");
            }

            try
            {
                p.Update(request.Name, request.Address, request.Price, request.CodeInternal, request.Year, request.IdOwner);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Uncreated Property");
                return Result.Failure(ex.Message);
            }
            _uow.Properties.Update(p);
            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Property successfully update for {PropertyId}", request.PropertyId);

            return Result.Success();
        }
    }
}