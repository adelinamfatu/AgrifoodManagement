using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Util.ValueObjects;
using AgrifoodManagement.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgrifoodManagement.Web.Controllers
{
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
                if (viewModel.Id == Guid.Empty) // Creating a new product
                {
                    var command = new CreateProductCommand
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Price = viewModel.Price,
                        Quantity = viewModel.Quantity,
                        UnitOfMeasurement = viewModel.UnitOfMeasurement,
                        ExpirationDate = viewModel.ExpirationDate,
                        ProductCategoryId = viewModel.Category?? 14,
                        TimePosted = DateTime.UtcNow,
                        AnnouncementStatus = AnnouncementStatus.Published,
                        IsPromoted = false,
                        UserId = new Guid("2346C2BF-1717-4AE3-9A69-F83B3A2D68FE")
                    };
                    Guid productId = await _mediator.Send(command);

                    return Ok(new
                    {
                        success = true,
                        redirectUrl = Url.Action("Admin", "Announcements", new { id = productId })
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
                        redirectUrl = Url.Action("Admin", "Announcements", new { id = viewModel.Id })
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error processing product: {ex.Message}");

                // Return error information
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }
    }
}
