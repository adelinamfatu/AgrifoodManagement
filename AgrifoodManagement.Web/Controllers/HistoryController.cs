using AgrifoodManagement.Business.Commands.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize]
    public class HistoryController : BaseUserController
    {
        public HistoryController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] Guid id)
        {
            var result = await _mediator.Send(new CancelOrderCommand(id));
            return result ? Ok() : BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Complete([FromBody] Guid id)
        {
            var result = await _mediator.Send(new CompleteOrderCommand(id));
            return result ? Ok() : BadRequest();
        }
    }
}
