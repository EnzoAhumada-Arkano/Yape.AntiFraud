using MediatR;
using Microsoft.AspNetCore.Mvc;
using Yape.Application.Transaction.Commands;
using Yape.Application.Transaction.Queries;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _mediator.Send(new GetAllTransactionsQuery());
            return Ok(transactions);
        }
        [HttpGet("GetByExternalId/{externalTransactionId}")]
        public async Task<IActionResult> GetByExternalId(Guid externalTransactionId)
        {
            var transaction = await _mediator.Send(new GetTransactionByExternalIdQuery()
            {
                TransactionExternalId = externalTransactionId
            });
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateTransactionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}
