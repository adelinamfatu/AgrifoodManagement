using AgrifoodManagement.Business.Commands.Forum;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models.Forum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize(Roles = "Seller")]
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
                return RedirectToAction("Producer/Forum");
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while creating the forum thread.");
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] AddCommentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid comment data." });
            }

            var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmailClaim))
            {
                return Json(new { success = false, message = "User email not found." });
            }

            var command = new AddCommentCommand
            {
                TopicId = viewModel.TopicId,
                CommentText = viewModel.CommentText,
                CreatedByUserEmail = userEmailClaim
            };

            CommentResultDto result = await _mediator.Send(command);

            return Json(result);
        }
    }
}
