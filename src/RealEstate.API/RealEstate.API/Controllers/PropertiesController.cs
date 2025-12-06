using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using RealEstate.Application.Commands;
using RealEstate.Application.Queries;
using RealEstate.Contracts.DTOs;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PropertiesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropertyRequest request)
        {
            var cmd = new CreatePropertyCommand(request.Name, request.Address, request.Price, request.CodeInternal, request.Year, request.IdOwner, request.ImageBase64);
            var result = await _mediator.Send(cmd);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return CreatedAtAction(nameof(GetById), new { id = result.Value!.IdProperty }, result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] Guid? ownerId, [FromQuery] int? year, [FromQuery] string? search)
        {
            var query = new GetPropertiesQuery(minPrice, maxPrice, ownerId, year, search);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            var dtos = _mapper.Map<IEnumerable<PropertyDto>>(result.Value);
            return Ok(dtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetPropertyByIdQuery(id);
            var result = await _mediator.Send(query);
            if (!result.IsSuccess) return NotFound(new { error = result.Error });
            var dto = _mapper.Map<PropertyDto>(result.Value!);
            return Ok(dto);
        }

        [HttpPatch("{id:guid}/price")]
        public async Task<IActionResult> ChangePrice(Guid id, [FromBody] ChangePriceRequest req)
        {
            var cmd = new ChangePriceCommand(id, req.NewPrice);
            var result = await _mediator.Send(cmd);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return NoContent();
        }

        [HttpPost("{id:guid}/images")]
        public async Task<IActionResult> AddImage(Guid id, [FromBody] AddImageRequest req)
        {
            var cmd = new AddPropertyImageCommand(id, req.ImageBase64);
            var result = await _mediator.Send(cmd);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePropertyRequest req)
        {
            var cmd = new UpdatePropertyCommand(id, req.Name, req.Address, req.Price, req.CodeInternal, req.Year, req.IdOwner);
            var result = await _mediator.Send(cmd);
            if (!result.IsSuccess) return BadRequest(new { error = result.Error });
            return NoContent();
        }
    }

    // Request DTOs
    public record CreatePropertyRequest(string Name, string Address, decimal Price, string CodeInternal, int Year, Guid IdOwner, string? ImageBase64);
    public record ChangePriceRequest(decimal NewPrice);
    public record AddImageRequest(string ImageBase64);
    public record UpdatePropertyRequest(string Name, string Address, decimal Price, string CodeInternal, int Year, Guid IdOwner);
}
