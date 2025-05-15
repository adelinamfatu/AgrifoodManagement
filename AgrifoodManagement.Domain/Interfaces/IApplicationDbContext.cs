using AgrifoodManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<ForumThread> ForumThreads { get; set; }
        DbSet<ForumPost> ForumPosts { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<WishlistItem> WishlistItems { get; set; }
        DbSet<DiscountCode> DiscountCodes { get; set; }
        DbSet<ExtendedProperty> ExtendedProperties { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
