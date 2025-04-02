using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Domain.Entities;
using MediatR;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public CreateProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var userId = _context.Users.Where(x => x.Email == request.Email)
                .FirstOrDefault()?
                .Id;

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                UnitOfMeasurement = request.UnitOfMeasurement,
                ExpirationDate = request.ExpirationDate,
                ProductCategoryId = request.ProductCategoryId,
                AnnouncementStatus = request.AnnouncementStatus,
                TimePosted = request.TimePosted,
                IsPromoted = request.IsPromoted,
                UserId = (Guid)userId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
