using AgrifoodManagement.Util.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class WishlistItemDto
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public MeasurementUnit? MeasurementUnit { get; init; }
        public string ImageUrl { get; init; }
    }
}
