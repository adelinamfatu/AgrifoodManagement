using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Order
{
    public class ConfirmOrderCommand : IRequest<Unit>
    {
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus FinalStatus { get; set; } = OrderStatus.Procesing;
        public string? DeliveryMethod { get; set; }
        public decimal? DeliveryFee { get; set; }
        public string? DiscountCode { get; set; }
        public string? DeliveryAddress { get; set; }
    }
}
