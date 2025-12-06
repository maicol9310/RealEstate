using MediatR;
using AutoMapper;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;
using RealEstate.Contracts.DTOs;

namespace RealEstate.Application.Features.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommandHandler : IRequestHandler<CreateOwnerCommand, Result<OwnerDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CreateOwnerCommandHandler(IUnitOfWork uow, IFileService fileService, IMapper mapper)
        {
            _uow = uow;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<Result<OwnerDto>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            string savedFileName = string.Empty;

            if (!string.IsNullOrWhiteSpace(request.Base64Photo))
            {
                try
                {
                    savedFileName = await _fileService.SaveBase64FileAsync(request.Base64Photo, cancellationToken);
                }
                catch (Exception ex)
                {
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

            return Result<OwnerDto>.Success(dto);
        }
    }
}
