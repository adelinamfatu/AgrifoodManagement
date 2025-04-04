using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgrifoodManagement.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> UpsertProduct(ProductViewModel viewModel)
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
                        redirectUrl = Url.Action("Announcements", "Admin")
                    });
                }
                else // Updating an existing product
                {
                    //var command = new UpdateProductCommand
                    //{
                    //    Id = viewModel.Id,
                    //    Name = viewModel.Name,
                    //    Description = viewModel.Description,
                    //    Price = viewModel.Price,
                    //    Quantity = viewModel.Quantity,
                    //    UnitOfMeasurement = viewModel.UnitOfMeasurement,
                    //    ExpirationDate = viewModel.ExpirationDate,
                    //    ProductCategoryId = viewModel.Category
                    //};
                    //await _mediator.Send(command);

                    return Ok(new
                    {
                        success = true,
                        redirectUrl = Url.Action("Announcements", "Admin")
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }
    }
}
