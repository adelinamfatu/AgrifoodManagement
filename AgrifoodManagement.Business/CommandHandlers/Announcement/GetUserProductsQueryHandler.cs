using AgrifoodManagement.Business.Queries.Product;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util.Models;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class GetUserProductsQueryHandler : IRequestHandler<GetUserProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetUserProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetUserProductsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.UserEmail, cancellationToken);

            return await _context.Products
                .Where(p => p.UserId == user!.Id)
                .Include(p => p.ProductCategory)
                .Include(p => p.OrderDetails)
                .Include(p => p.WishlistItems)
                .Include(p => p.Reviews)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CurrentPrice = p.CurrentPrice ?? p.OriginalPrice,
                    Quantity = p.Quantity,
                    UnitOfMeasurement = p.UnitOfMeasurement,
                    ExpirationDate = p.ExpirationDate,
                    CategoryId = p.ProductCategoryId,
                    CategoryName = p.ProductCategory != null ? p.ProductCategory.Name : "Uncategorized",
                    CartQuantity = p.OrderDetails.Where(od => od.Order!.Status == OrderStatus.InCart
                        || od.Order.Status == OrderStatus.Pending)
                        .Sum(od => od.Quantity),
                    WishlistQuantity = p.WishlistItems.Count(),
                    MajoritySentiment = p.Reviews.Any()
                        ? p.Reviews
                            .GroupBy(r => r.SentimentType)
                            .OrderByDescending(g => g.Count())
                            .Select(g => g.Key)
                            .First()
                        : (SentimentType?)null,
                    SentimentConfidence = p.Reviews.Any()
                        ? Math.Round(p.Reviews.Average(r => (double)r.SentimentConfidence), 2)
                        : (double?)null,
                    IsPromoted = p.IsPromoted,
                    AnnouncementStatus = p.AnnouncementStatus,
                    PhotoUrls = _context.ExtendedProperties
                        .Where(ep => ep.EntityId == p.Id && ep.Key == "PhotoUrl")
                        .Select(ep => ep.Value)
                        .ToList()
                })
                .ToListAsync(cancellationToken);
        }
    }
}
