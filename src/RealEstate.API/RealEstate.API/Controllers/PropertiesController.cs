using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Commands;
using RealEstate.Application.Queries;
using RealEstate.Contracts.DTOs;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PropertiesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var res = await _mediator.Send(new GetPropertiesQuery());
            if (!res.IsSuccess) return BadRequest(res.Error);
            var dtos = _mapper.Map<IEnumerable<PropertyDto>>(res.Value);
            return Ok(dtos);
        }

    }

    // DTOs for incoming requests in API layer (these are different from Contracts)
    public record CreatePropertyDto(string Name, string Address, decimal Price, string CodeInternal, int Year, string IdOwner, string? ImageBase64);
    public record ChangePriceDto(decimal NewPrice);
}
