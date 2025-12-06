using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Commands;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class AddPropertyImageHandler : IRequestHandler<AddPropertyImageCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;

        public AddPropertyImageHandler(IUnitOfWork uow, IFileService fileService)
        {
            _uow = uow;
            _fileService = fileService;
        }

        public async Task<Result> Handle(AddPropertyImageCommand request, CancellationToken cancellationToken)
        {
            var p = await _uow.Properties.GetByIdAsync(request.PropertyId, cancellationToken);
            if (p == null) return Result.Failure("Property not found");
            var path = await _fileService.SaveBase64FileAsync(request.ImageBase64, cancellationToken);
            var img = new PropertyImage(p.IdProperty, path);
            p.AddImage(img);
            _uow.Properties.Update(p);
            await _uow.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}