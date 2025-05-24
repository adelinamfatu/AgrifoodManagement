using AgrifoodManagement.Util.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Services.Interfaces
{
    public interface IOrderStatusTransitionValidator
    {
        bool IsValidTransition(OrderStatus oldStatus, OrderStatus newStatus);
    }
}
