using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Interfaces;
using RealEstate.SharedKernel;
using RealEstate.Contracts.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Application.Commands;

namespace RealEstate.Application.Handlers
{
    public class CreateOwnerCommandHandler : IRequestHandler<CreateOwnerCommand, Result<OwnerDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOwnerCommandHandler> _logger;

        public CreateOwnerCommandHandler(IUnitOfWork uow, IFileService fileService, IMapper mapper, ILogger<CreateOwnerCommandHandler> logger)
        {
            _uow = uow;
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<OwnerDto>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing Owner creation request for {Name}", request.Name);

            string savedFileName = string.Empty;

            if (!string.IsNullOrWhiteSpace(request.Base64Photo))
            {
                try
                {
                    savedFileName = await _fileService.SaveBase64FileAsync(request.Base64Photo, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Error saving photo");
                    return Result<OwnerDto>.Failure($"Error saving photo: {ex.Message}");
                }
            }

            Owner owner;
            try
            {
                owner = new Owner(request.Name, request.Address, savedFileName, request.Birthday);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Uncreated owner");
                return Result<OwnerDto>.Failure(ex.Message);
            }

            await _uow.Owners.AddAsync(owner, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            var dto = _mapper != null
                ? _mapper.Map<OwnerDto>(owner)
                : new OwnerDto
                {
                    IdOwner = owner.IdOwner,
                    Name = owner.Name,
                    Address = owner.Address,
                    Photo = owner.Photo,
                    Birthday = owner.Birthday
                };

            _logger.LogInformation("Owner successfully created for {Name}", request.Name);

            return Result<OwnerDto>.Success(dto);
        }
    }
}
