using AutoMapper;
using MediatR;
using RealEstate.Application.Commands;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Shared;
using RealEstate.Contracts.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class CreatePropertyImageHandler : IRequestHandler<CreatePropertyImageCommand, Result<PropertyImageDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CreatePropertyImageHandler(IUnitOfWork uow, IFileService fileService, IMapper mapper)
        {
            _uow = uow;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<Result<PropertyImageDto>> Handle(CreatePropertyImageCommand request, CancellationToken cancellationToken)
        {
            PropertyImage img;

            var p = await _uow.Properties.GetByIdAsync(request.PropertyId, cancellationToken);
            if (p == null) return Result<PropertyImageDto>.Failure("Property not found");

            try
            {
                var path = await _fileService.SaveBase64FileAsync(request.ImageBase64, cancellationToken);
                img = new PropertyImage(request.PropertyId, path);
            } 
            catch (ArgumentException ex)
            {
                return Result<PropertyImageDto>.Failure(ex.Message);
            }
    
            await _uow.PropertyImage.AddAsync(img, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<PropertyImageDto>(img);
            return Result<PropertyImageDto>.Success(dto);
        }
    }
}
