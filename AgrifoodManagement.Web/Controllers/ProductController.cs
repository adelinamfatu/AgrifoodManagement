using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models;
using AgrifoodManagement.Web.Models.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize(Roles = "Seller")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IStripeCheckoutService _stripeService;

        public ProductController(IMediator mediator, IStripeCheckoutService stripeService)
        {
            _mediator = mediator;
            _stripeService = stripeService;
        }

        [HttpPost]
        public async Task<IActionResult> UpsertProduct(UpsertProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                              .ToDictionary(
                                  kvp => kvp.Key,
                                  kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                              )
                });
            }

            try
            {
                var uploadedPhotos = Request.Form.Files.ToList();

                if (viewModel.Id == Guid.Empty) // Creating a new product
                {
                    var userEmailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

                    var command = new CreateProductCommand
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Price = viewModel.Price,
                        Quantity = viewModel.Quantity,
                        UnitOfMeasurement = viewModel.UnitOfMeasurement,
                        ExpirationDate = viewModel.ExpirationDate,
                        ProductCategoryId = viewModel.Category,
                        TimePosted = DateTime.UtcNow,
                        AnnouncementStatus = AnnouncementStatus.Published,
                        IsPromoted = false,
                        PhotoUrls = viewModel.PhotoUrls,
                        Email = userEmailClaim
                    };

                    Guid productId = await _mediator.Send(command);

                    var photoCommand = new UploadProductPhotoCommand
                    {
                        Photos = uploadedPhotos,
                        ProductId = productId,
                        Folder = PhotoFolder.Products
                    };

                    var result = await _mediator.Send(photoCommand);

                    return Ok(new
                    {
                        success = true,
                        redirectUrl = Url.Action("Announcements", "Producer")
                    });
                }
                else // Updating an existing product
                {
                    var command = new UpdateProductCommand
                    {
                        Id = viewModel.Id,
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        UnitOfMeasurement = viewModel.UnitOfMeasurement,
                        ExpirationDate = viewModel.ExpirationDate,
                        ProductCategoryId = (int)viewModel.Category
                    };
                    await _mediator.Send(command);

                    if (uploadedPhotos.Any())
                    {
                        await _mediator.Send(new DeleteProductPhotosCommand { ProductId = viewModel.Id });
                        
                        var photoCmd = new UploadProductPhotoCommand
                        {
                            ProductId = viewModel.Id,
                            Photos = uploadedPhotos,
                            Folder = PhotoFolder.Products
                        };
                        await _mediator.Send(photoCmd);
                    }

                    return Ok(new
                    {
                        success = true,
                        redirectUrl = Url.Action("Announcements", "Producer")
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStock([FromBody] ProductViewModel model)
        {
            var command = new UpdateProductStockCommand
            {
                Id = model.Id,
                Quantity = model.Quantity,
                CurrentPrice = model.CurrentPrice
            };

            try
            {
                await _mediator.Send(command);
                return Ok(new
                {
                    message = "Stock updated successfully.",
                    toastType = "success"
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    toastType = "warning"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message,
                    toastType = "error"
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred.",
                    toastType = "error"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Promote([FromBody] PromoteProductDto dto)
        {
            var items = new[]
            {
                new StripeLineItemDto {
                    Name        = $"Promote “{dto.ProductName}”",
                    Description = "Boost your listing",
                    UnitAmount  = 2500,
                    Currency    = "ron",
                    Quantity    = 1
                }
            };

            var domain = $"{Request.Scheme}://{Request.Host}";
            var successUrl = $"{domain}/Product/CompletePromotion?productId={dto.ProductId}&session_id={{CHECKOUT_SESSION_ID}}";
            var cancelUrl = $"{domain}/Producer/Announcements";

            var result = await _stripeService.CreateCheckoutSessionAsync(
                lineItems: items,
                successUrl: successUrl,
                cancelUrl: cancelUrl,
                metadata: new Dictionary<string, string>
                {
                    ["ProductId"] = dto.ProductId.ToString()
                }
            );

            if (!result.Success)
                return BadRequest(new { success = false, error = result.ErrorMessage });

            return Ok(new { 
                success = true, 
                sessionId = result.SessionId, 
                publishableKey = result.PublishableKey });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CompletePromotion(Guid productId, string session_id)
        {
            var service = new Stripe.Checkout.SessionService();
            var session = await service.GetAsync(session_id);
            if (session.PaymentStatus != "paid")
            {
                TempData["Error"] = "Payment not completed.";
                return RedirectToAction("Announcements");
            }

            // Mark the product as promoted & record in ExtendedProperty
            var ok = await _mediator.Send(new PromoteProductCommand(productId, session_id));
            if (!ok)
            {
                TempData["Error"] = "Could not finalize promotion.";
            }
            else
            {
                TempData["Success"] = "Your product is now promoted!";
            }

            return RedirectToAction("Producer/Announcements");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductStatus([FromBody] UpdateProductStatusDto dto)
        {
            var ok = await _mediator.Send(new UpdateProductStatusCommand(dto.ProductId, dto.NewStatus));

            return ok ? Ok() : BadRequest("Could not update product status");
        }
    }
}
