using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize]
    public class HistoryController : BaseUserController
    {
        public HistoryController(IMediator mediator) 
            : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDto dto)
        {
            var ok = await _mediator.Send(new UpdateOrderStatusCommand(dto.OrderId, dto.NewStatus));

            return ok ? Ok() : BadRequest("Could not update status");
        }
    }
}
