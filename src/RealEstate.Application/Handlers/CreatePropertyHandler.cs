using MediatR;
using RealEstate.Application.Commands;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class CreatePropertyHandler : IRequestHandler<CreatePropertyCommand, Result<Property>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;

        public CreatePropertyHandler(IUnitOfWork uow, IFileService fileService)
        {
            _uow = uow;
            _fileService = fileService;
        }

        public async Task<Result<Property>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            if (request.Price <= 0) return Result<Property>.Failure("Price must be greater than 0");
            if (request.IdOwner == Guid.Empty) return Result<Property>.Failure("Owner required");

            var prop = new Property(request.Name, request.Address, request.Price, request.CodeInternal, request.Year, request.IdOwner);

            if (!string.IsNullOrEmpty(request.ImageBase64))
            {
                var path = await _fileService.SaveBase64FileAsync(request.ImageBase64, cancellationToken);
                prop.AddImage(new PropertyImage(prop.IdProperty, path));
            }

            await _uow.Properties.AddAsync(prop, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Result<Property>.Success(prop);
        }
    }
}