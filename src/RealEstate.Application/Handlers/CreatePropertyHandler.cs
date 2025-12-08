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
    public class CreatePropertyHandler : IRequestHandler<CreatePropertyCommand, Result<PropertyDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePropertyHandler> _logger;

        public CreatePropertyHandler(IUnitOfWork uow, IFileService fileService, IMapper mapper, ILogger<CreatePropertyHandler> logger)
        {
            _uow = uow;
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PropertyDto>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing property creation request for {Name}", request.Name);

            Property prop;
            try
            {
                prop = new Property(request.Name, request.Address, request.Price, request.CodeInternal, request.Year, request.IdOwner);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Uncreated property");
                return Result<PropertyDto>.Failure(ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                var path = await _fileService.SaveBase64FileAsync(request.ImageBase64, cancellationToken);
                prop.AddImage(new PropertyImage(prop.IdProperty, path));
            }

            _logger.LogInformation("Property successfully created for {Name}", request.Name);
            await _uow.Properties.AddAsync(prop, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<PropertyDto>(prop);
            return Result<PropertyDto>.Success(dto);
        }
    }
}
