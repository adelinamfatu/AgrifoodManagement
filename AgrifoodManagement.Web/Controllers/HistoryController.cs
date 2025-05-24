using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var cmd = new AddReviewCommand(email, dto.ProductId, dto.Rating, dto.Comment);
            var success = await _mediator.Send(cmd);
            return Ok(new { success });
        }
    }
}
