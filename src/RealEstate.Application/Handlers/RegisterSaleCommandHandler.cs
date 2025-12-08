using AutoMapper;
using MediatR;
using RealEstate.Application.Commands;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Shared;
using RealEstate.Contracts.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers
{
    public class RegisterSaleCommandHandler : IRequestHandler<RegisterSaleCommand, Result<PropertyTraceDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RegisterSaleCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<PropertyTraceDto>> Handle(RegisterSaleCommand request, CancellationToken cancellationToken)
        {
            PropertyTrace trace;

            try
            {
               trace = new PropertyTrace(request.dateSale, request.name, request.value, request.tax, request.idProperty);
            }
            catch (ArgumentException ex)
            {
                return Result<PropertyTraceDto>.Failure(ex.Message);
            }

            await _uow.PropertyTrace.AddAsync(trace, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<PropertyTraceDto>(trace);
            return Result<PropertyTraceDto>.Success(dto);
        }
    }
}
