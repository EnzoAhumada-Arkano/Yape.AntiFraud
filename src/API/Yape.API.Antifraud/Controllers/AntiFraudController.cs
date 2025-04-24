using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Yape.Application.Antifraud.Commands;

namespace Yape.Antifraud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AntiFraudController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AntiFraudController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Validate")]
        public async Task<IActionResult> ValidateTransaction([FromBody] ValidateTransactionCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid command");
            }
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
