using System;
using System.Net;
using System.Threading.Tasks;
using CQRS.Api.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CQRSController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CQRSController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromHeader(Name = "x-requestid")] string requestId, [FromBody] CreateDomainCommand command)
        {
            bool commandResult = false;
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var createCQRS = new IdentifiedCommand<CreateDomainCommand, bool>(command, guid);
                commandResult = await _mediator.Send(createCQRS);
            }
            return commandResult ? (IActionResult)Ok() : (IActionResult)BadRequest();
        }
    }
}
