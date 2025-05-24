using AgrifoodManagement.Business.Services.Interfaces;
using AgrifoodManagement.Util.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Impl
{
    public class OrderStatusTransitionValidator : IOrderStatusTransitionValidator
    {
        public bool IsValidTransition(OrderStatus oldStatus, OrderStatus newStatus)
        {
            return (oldStatus, newStatus) switch
            {
                (OrderStatus.InCart, OrderStatus.Pending) => true,
                (OrderStatus.Pending, OrderStatus.Processing) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Processing, OrderStatus.Canceled) => true,
                (OrderStatus.Shipped, OrderStatus.Completed) => true,
                _ => false
            };
        }
    }
}
