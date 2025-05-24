using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Cart
{
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public AddProductToCartCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var buyer = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.BuyerEmail, cancellationToken);

                if (buyer == null)
                    throw new Exception("User not found");

                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.BuyerId == buyer.Id && o.Status == OrderStatus.InCart, cancellationToken);

                if (order == null)
                {
                    order = new Order
                    {
                        Id = Guid.NewGuid(),
                        BuyerId = buyer.Id,
                        Status = OrderStatus.InCart,
                        OrderedAt = null,
                        TotalAmount = 0,
                        PhoneNumber = "",
                        DeliveryAddress = "",
                        OrderDetails = new List<OrderDetail>()
                    };

                    _context.Orders.Add(order);
                }

                var product = await _context.Products
                    .FindAsync(new object[] { request.ProductId }, cancellationToken);

                if (product == null)
                    throw new Exception("Product not found");

                var unitPrice = product.CurrentPrice ?? product.OriginalPrice;

                var existingItem = order.OrderDetails
                    .FirstOrDefault(od => od.ProductId == request.ProductId
                                    && od.Id == order.Id);

                if (existingItem != null)
                {
                    existingItem.Quantity += request.Quantity;
                }
                else
                {
                    var newDetail = new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Quantity = request.Quantity,
                        UnitPrice = unitPrice,
                        SellerId = product.UserId
                    };

                    order.OrderDetails.Add(newDetail);
                }

                order.TotalAmount = order.OrderDetails.Sum(od => od.UnitPrice * od.Quantity);

                await _context.SaveChangesAsync(cancellationToken);

                return order.Id;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("The cart could not be updated because it was changed by another process. Please try again.", ex);
            }
            catch (Exception)
            {
                throw new Exception("An unexpected error occurred while adding the product to the cart. Please try again.");
            }
        }
    }
}
