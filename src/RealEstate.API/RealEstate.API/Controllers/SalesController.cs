using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Commands;
using RealEstate.Application.Features.Owners.Commands.CreateOwner;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegisterSaleRequest request)
        {
            var cmd = new RegisterSaleCommand(
                request.dateSale,
                request.name,
                request.value,
                request.tax,
                request.idProperty
            );

            var result = await _mediator.Send(cmd);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Error });

            return CreatedAtAction(nameof(Create), new { id = result.Value!.IdPropertyTrace}, result.Value);
        }
    }

    public record RegisterSaleRequest(DateTime dateSale, string name, decimal value, decimal tax, Guid idProperty);
}