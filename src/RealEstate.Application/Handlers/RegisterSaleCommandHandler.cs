using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RealEstate.Application.Commands;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Shared;
using RealEstate.Contracts.DTOs;
using RealEstate.Domain.Entities;
using Serilog;

namespace RealEstate.Application.Handlers
{
    public class RegisterSaleCommandHandler : IRequestHandler<RegisterSaleCommand, Result<PropertyTraceDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterSaleCommandHandler> _logger;

        public RegisterSaleCommandHandler(IUnitOfWork uow, IMapper mapper, ILogger<RegisterSaleCommandHandler> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PropertyTraceDto>> Handle(RegisterSaleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing sales creation request for {name}", request.name);

            PropertyTrace trace;

            try
            {
               trace = new PropertyTrace(request.dateSale, request.name, request.value, request.tax, request.idProperty);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Uncreated sale");
                return Result<PropertyTraceDto>.Failure(ex.Message);
            }

            await _uow.PropertyTrace.AddAsync(trace, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Sale successfully created for {idProperty}", request.idProperty);

            var dto = _mapper.Map<PropertyTraceDto>(trace);
            return Result<PropertyTraceDto>.Success(dto);
        }
    }
}
