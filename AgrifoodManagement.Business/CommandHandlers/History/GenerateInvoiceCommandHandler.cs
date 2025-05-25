using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.History
{
    public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, byte[]>
    {
        private readonly ApplicationDbContext _context;
        private readonly IInvoiceService _pdf;

        public GenerateInvoiceCommandHandler(ApplicationDbContext context, IInvoiceService pdf)
        {
            _context = context;
            _pdf = pdf;
        }

        public async Task<byte[]> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Buyer)
                .Include(o => o.OrderDetails)
                   .ThenInclude(d => d.Product)
                   .ThenInclude(d => d.Seller)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException($"Order {request.OrderId} not found.");

            var sections = order.OrderDetails
                .GroupBy(d => d.Product.Seller!)
                .Select(async g => {
                    var seller = g.Key;
                    var sig = await _context.ExtendedProperties
                        .Where(ep => ep.EntityType == "User"
                                  && ep.EntityId == seller.Id
                                  && ep.Key == "SignatureUrl")
                        .Select(ep => ep.Value)
                        .FirstOrDefaultAsync(cancellationToken);

                    return new SellerInvoiceSectionDto
                    {
                        SellerName = $"{seller.FirstName} {seller.LastName}",
                        SellerAddress = seller.Address ?? "",
                        SellerSignatureDataUrl = sig,
                        Items = g.Select(d => new InvoiceLineItemDto
                        {
                            ProductName = d.Product!.Name,
                            Quantity = d.Quantity,
                            UnitPrice = d.UnitPrice
                        }).ToList()
                    };
                })
                .Select(t => t.Result)
                .ToList();

            var dto = new InvoiceDataDto
            {
                OrderId = order.Id,
                OrderDate = order.OrderedAt!.Value,
                ConsumerName = $"{order.Buyer!.FirstName} {order.Buyer.LastName}",
                ConsumerAddress = order.DeliveryAddress!,
                Sellers = sections
            };

            return await _pdf.GenerateInvoiceAsync(dto);
        }
    }
}
