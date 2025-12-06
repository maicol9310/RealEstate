using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Commands;
using RealEstate.Application.Shared;

namespace RealEstate.Application.Handlers
{
    public class ChangePriceHandler : IRequestHandler<ChangePriceCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public ChangePriceHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(ChangePriceCommand request, CancellationToken cancellationToken)
        {
            var p = await _uow.Properties.GetByIdAsync(request.PropertyId, cancellationToken);
            if (p == null) return Result.Failure("Property not found");
            try
            {
                p.ChangePrice(request.NewPrice);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure(ex.Message);
            }
            _uow.Properties.Update(p);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}