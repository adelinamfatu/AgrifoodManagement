using AgrifoodManagement.Business.Commands.Forum;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models.Forum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize]
    public class ForumController : Controller
    {
        private readonly IMediator _mediator;

        public ForumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddForumThread([FromBody] CreateForumThreadViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
            if (!Enum.TryParse<ForumCategory>(viewModel.Category, true, out var categoryEnum))
            {
                return BadRequest("Invalid category.");
            }

            var command = new CreateForumThreadCommand
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                Category = categoryEnum,
                CreatedByUserEmail = userEmailClaim
            };

            bool isSuccess = await _mediator.Send(command);

            if (isSuccess)
            {
                return RedirectToAction("Admin/Forum");
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while creating the forum thread.");
                return View(viewModel);
            }
        }
    }
}
