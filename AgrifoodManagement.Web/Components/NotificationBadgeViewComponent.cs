using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Business.Queries.Order;
using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Components
{
    public class NotificationBadgeViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public NotificationBadgeViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string badgeFor)
        {
            var email = ((ClaimsPrincipal)User).FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return Content(string.Empty);

            int count = 0;

            switch (badgeFor)
            {
                case "Cart":
                    var cartDto = await _mediator.Send(new GetCartByEmailQuery { BuyerEmail = email });
                    count = cartDto.Items?.Sum(i => i.QuantityOrdered) ?? 0;
                    break;

                case "Favorites":
                    var wishlistDto = await _mediator.Send(new GetUserWishlistQuery(email));
                    count = wishlistDto.Count();
                    break;
            }

            var viewModel = new NotificationBadgeViewModel
            {
                Count = count,
                Color = "e-badge-danger",
                PositionTop = "0",
                PositionRight = "0"
            };

            return View("~/Views/Shared/NotificationBadge.cshtml", viewModel);
        }
    }
}
