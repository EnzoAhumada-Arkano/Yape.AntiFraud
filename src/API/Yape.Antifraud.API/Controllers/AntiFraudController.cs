using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
