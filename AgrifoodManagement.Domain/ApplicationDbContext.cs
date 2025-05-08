using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ForumThread> ForumThreads { get; set; }

        public DbSet<ForumPost> ForumPosts { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<WishlistItem> WishlistItems { get; set; }

        public DbSet<DiscountCode> DiscountCodes { get; set; }

        public DbSet<ExtendedProperty> ExtendedProperties { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evade on delete cascade
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Seller)
                .WithMany()
                .HasForeignKey(od => od.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.ParentCategory)
                .WithMany(pc => pc.SubCategories)
                .HasForeignKey(pc => pc.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany()
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ForumThread>()
                .HasMany(ft => ft.Posts)
                .WithOne(fp => fp.Thread)
                .HasForeignKey(fp => fp.ThreadId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.ReviewsWritten)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ReviewedUser)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(r => r.ReviewedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Set exactly 2 decimals
            modelBuilder.Entity<Product>()
                .Property(p => p.OriginalPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.CurrentPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
                .Property(p => p.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.DeliveryFee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<DiscountCode>()
                .Property(dc => dc.Value)
                .HasPrecision(18, 2);

            // Create indexes
            modelBuilder.Entity<ExtendedProperty>()
                .HasIndex(ep => new { ep.EntityId, ep.EntityType });

            modelBuilder.Entity<ForumPost>()
                .HasIndex(fp => fp.PostedAt);

            // Unicity
            modelBuilder.Entity<WishlistItem>()
                .HasIndex(w => new { w.UserId, w.ProductId })
                .IsUnique();
        }
    }
}
