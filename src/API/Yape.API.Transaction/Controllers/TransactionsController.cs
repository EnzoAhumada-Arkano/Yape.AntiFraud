using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransaction(Guid transactionId)
        {
            var transaction = await _mediator.Send(new GetTransactionByIdQuery()
            {
                TransactionExternalId = transactionId
            });
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTransactionCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}
