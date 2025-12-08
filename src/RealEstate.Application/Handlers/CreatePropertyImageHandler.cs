using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Commands;
using RealEstate.Application.Interfaces;
using RealEstate.SharedKernel;
using RealEstate.Contracts.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class CreatePropertyImageHandler : IRequestHandler<CreatePropertyImageCommand, Result<PropertyImageDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePropertyImageHandler> _logger;

        public CreatePropertyImageHandler(IUnitOfWork uow, IFileService fileService, IMapper mapper, ILogger<CreatePropertyImageHandler> logger)
        {
            _uow = uow;
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PropertyImageDto>> Handle(CreatePropertyImageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing image creation request for {PropertyId}", request.PropertyId);

            PropertyImage img;

            var p = await _uow.Properties.GetByIdAsync(request.PropertyId, cancellationToken);
            if (p == null)
            {
                _logger.LogWarning("Property not found");
                return Result<PropertyImageDto>.Failure("Property not found");
            }

            try
            {
                var path = await _fileService.SaveBase64FileAsync(request.ImageBase64, cancellationToken);
                img = new PropertyImage(request.PropertyId, path);
            } 
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Uncreated image");
                return Result<PropertyImageDto>.Failure(ex.Message);
            }
    
            await _uow.PropertyImage.AddAsync(img, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Image successfully created for {PropertyId}", request.PropertyId);

            var dto = _mapper.Map<PropertyImageDto>(img);
            return Result<PropertyImageDto>.Success(dto);
        }
    }
}
