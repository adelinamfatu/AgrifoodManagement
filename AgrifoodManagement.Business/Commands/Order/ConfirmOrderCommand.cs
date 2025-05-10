using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Order
{
    public class ConfirmOrderCommand : IRequest<Guid>
    {
        public Guid OrderId { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public decimal TotalAmount { get; set; }
        public string? DeliveryMethod { get; set; }
        public decimal? DeliveryFee { get; set; }
        public string? DiscountCode { get; set; }
        public string? DeliveryAddress { get; set; }
    }
}
