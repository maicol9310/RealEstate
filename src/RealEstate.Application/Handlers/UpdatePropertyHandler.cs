using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Commands;
using RealEstate.Application.Shared;

namespace RealEstate.Application.Handlers
{
    public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public UpdatePropertyHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var p = await _uow.Properties.GetByIdAsync(request.PropertyId, cancellationToken);
            if (p == null) return Result.Failure("Property not found");
            try
            {
                p.Update(request.Name, request.Address, request.Price, request.CodeInternal, request.Year, request.IdOwner);
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