using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.ValueObjects
{
    public enum OrderStatus
    {
        InCart,
        Pending,
        Procesing,
        Shipped,
        Canceled,
        Completed
    }
}
