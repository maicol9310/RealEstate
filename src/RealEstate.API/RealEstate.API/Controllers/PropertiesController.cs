using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Commands;
using RealEstate.Application.Queries;
using RealEstate.Contracts.DTOs;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireUser")]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertiesController> _logger;

        public PropertiesController(IMediator mediator, IMapper mapper, ILogger<PropertiesController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropertyRequest request)
        {
            _logger.LogInformation("Creating property for owner {OwnerId}", request.IdOwner);
            var cmd = new CreatePropertyCommand(
                request.Name,
                request.Address,
                request.Price,
                request.CodeInternal,
                request.Year,
                request.IdOwner,
                request.ImageBase64
            );

            try
            {
                var result = await _mediator.Send(cmd);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to create property: {Error}", result.Error);
                    return BadRequest(new { error = result.Error });
                }

                _logger.LogInformation("Property created successfully: {PropertyId}", result.Value!.IdProperty);
                return CreatedAtAction(nameof(GetById), new { id = result.Value.IdProperty }, result.Value);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed while creating property: {Errors}", ex.Errors.Select(e => e.ErrorMessage));
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { IsSuccess = false, Errors = errors });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] Guid? ownerId, [FromQuery] int? year)
        {
            _logger.LogInformation("Fetching properties with filters: minPrice={MinPrice}, maxPrice={MaxPrice}, ownerId={OwnerId}, year={Year}",
                minPrice, maxPrice, ownerId, year);

            var query = new GetPropertiesQuery(minPrice, maxPrice, ownerId, year);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to fetch properties: {Error}", result.Error);
                return BadRequest(new { error = result.Error });
            }

            var dtos = _mapper.Map<IEnumerable<PropertyDto>>(result.Value);
            return Ok(dtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation("Fetching property by Id: {PropertyId}", id);
            var query = new GetPropertyByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Property not found: {PropertyId}", id);
                return NotFound(new { error = result.Error });
            }

            var dto = _mapper.Map<PropertyDto>(result.Value!);
            return Ok(dto);
        }

        [HttpPatch("{id:guid}/price")]
        public async Task<IActionResult> ChangePrice(Guid id, [FromBody] ChangePriceRequest req)
        {
            _logger.LogInformation("Changing price for property {PropertyId} to {NewPrice}", id, req.NewPrice);
            var cmd = new ChangePriceCommand(id, req.NewPrice);

            try
            {
                var result = await _mediator.Send(cmd);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to change price for property {PropertyId}: {Error}", id, result.Error);
                    return BadRequest(new { error = result.Error });
                }

                _logger.LogInformation("Price changed successfully for property {PropertyId}", id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed while changing price for property {PropertyId}: {Errors}", id, ex.Errors.Select(e => e.ErrorMessage));
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { IsSuccess = false, Errors = errors });
            }
        }

        [HttpPost("{id:guid}/images")]
        public async Task<IActionResult> CreateImage(Guid id, [FromBody] AddImageRequest req)
        {
            _logger.LogInformation("Adding image to property {PropertyId}", id);
            var cmd = new CreatePropertyImageCommand(id, req.ImageBase64);

            try
            {
                var result = await _mediator.Send(cmd);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to add image to property {PropertyId}: {Error}", id, result.Error);
                    return BadRequest(new { error = result.Error });
                }

                _logger.LogInformation("Image added successfully to property {PropertyId}", id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed while adding image to property {PropertyId}: {Errors}", id, ex.Errors.Select(e => e.ErrorMessage));
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { IsSuccess = false, Errors = errors });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePropertyRequest req)
        {
            _logger.LogInformation("Updating property {PropertyId}", id);
            var cmd = new UpdatePropertyCommand(
                id,
                req.Name,
                req.Address,
                req.Price,
                req.CodeInternal,
                req.Year,
                req.IdOwner
            );

            try
            {
                var result = await _mediator.Send(cmd);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to update property {PropertyId}: {Error}", id, result.Error);
                    return BadRequest(new { error = result.Error });
                }

                _logger.LogInformation("Property updated successfully {PropertyId}", id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed while updating property {PropertyId}: {Errors}", id, ex.Errors.Select(e => e.ErrorMessage));
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { IsSuccess = false, Errors = errors });
            }
        }
    }

    public record ChangePriceRequest(decimal NewPrice);
    public record AddImageRequest(string ImageBase64);
    public record UpdatePropertyRequest(string Name, string Address, decimal Price, string CodeInternal, int Year, Guid IdOwner);
    public record CreatePropertyRequest(string Name, string Address, decimal Price, string CodeInternal, int Year, Guid IdOwner, string ImageBase64);
}