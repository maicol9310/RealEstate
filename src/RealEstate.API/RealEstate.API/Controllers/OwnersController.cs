using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Commands;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireUser")]
    public class OwnersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OwnersController(IMediator mediator)
        {
            _mediator = mediator;
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

            try
            {
                var result = await _mediator.Send(cmd);

                if (!result.IsSuccess)
                    return BadRequest(new { error = result.Error });

                return CreatedAtAction(nameof(Create), new { id = result.Value!.IdOwner }, result.Value);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { IsSuccess = false, Errors = errors });
            }
        }
    }

    public record CreateOwnerRequest(string Name, string Address, string? Base64Photo, DateTime Birthday);
}