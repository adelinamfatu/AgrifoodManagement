using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.ValueObjects
{
    public class OrderTreeDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Delivery { get; set; } = "";
        public string Status { get; set; } = "";
        public string Quantity { get; set; }
        public decimal? Total { get; set; }
        public List<OrderTreeDto>? Children { get; set; }
    }
}
