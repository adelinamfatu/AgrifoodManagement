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
        DbSet<Product> Products { get; }
        DbSet<User> Users { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderDetail> OrderDetails { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
