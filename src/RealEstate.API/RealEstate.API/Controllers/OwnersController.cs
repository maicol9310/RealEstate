using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using RealEstate.Application.Features.Owners.Commands.CreateOwner;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OwnersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOwnerRequest request)
        {
            var cmd = new CreateOwnerCommand(
                request.Name,
                request.Address,
                request.Base64Photo,
                request.Birthday
            );

            var result = await _mediator.Send(cmd);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return CreatedAtAction(nameof(Create), new { id = result.Value!.IdOwner }, result.Value);
        }
    }

    public record CreateOwnerRequest(string Name, string Address, string? Base64Photo, DateTime Birthday);
}