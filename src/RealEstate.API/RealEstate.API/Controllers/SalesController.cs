using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Commands;

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

            try
            {
                var result = await _mediator.Send(cmd);

                if (!result.IsSuccess)
                    return BadRequest(new { error = result.Error });

                return CreatedAtAction(nameof(Create), new { id = result.Value!.IdPropertyTrace }, result.Value);
            }
            catch (ValidationException ex)
            {
                // Captura errores de FluentValidation
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { IsSuccess = false, Errors = errors });
            }
        }
    }

    public record RegisterSaleRequest(DateTime dateSale, string name, decimal value, decimal tax, Guid idProperty);
}