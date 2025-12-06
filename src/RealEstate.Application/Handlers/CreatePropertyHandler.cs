using MediatR;
using AutoMapper;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Commands;
using RealEstate.Application.Shared;
using RealEstate.Domain.Entities;
using RealEstate.Contracts.DTOs;

namespace RealEstate.Application.Handlers
{
    public class CreatePropertyHandler : IRequestHandler<CreatePropertyCommand, Result<PropertyDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CreatePropertyHandler(IUnitOfWork uow, IFileService fileService, IMapper mapper)
        {
            _uow = uow;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<Result<PropertyDto>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            Property prop;
            try
            {
                prop = new Property(request.Name, request.Address, request.Price, request.CodeInternal, request.Year, request.IdOwner);
            }
            catch (ArgumentException ex)
            {
                return Result<PropertyDto>.Failure(ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                var path = await _fileService.SaveBase64FileAsync(request.ImageBase64, cancellationToken);
                prop.AddImage(new PropertyImage(prop.IdProperty, path));
            }

            await _uow.Properties.AddAsync(prop, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<PropertyDto>(prop);
            return Result<PropertyDto>.Success(dto);
        }
    }
}
