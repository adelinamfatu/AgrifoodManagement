using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize]
    [Route("Admin/Forum")]
    public class ForumController : Controller
    {
        private readonly IMediator _mediator;

        public ForumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("AddDiscussion")]
        public IActionResult AddDiscussion()
        {
            return View("~/Views/Admin/AddDiscussion.cshtml");
        }
    }
}
