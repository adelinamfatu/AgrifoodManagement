using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Product
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public MeasurementUnit? UnitOfMeasurement { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int? ProductCategoryId { get; set; }
        public AnnouncementStatus AnnouncementStatus { get; set; }
        public DateTime? TimePosted { get; set; }
        public bool IsPromoted { get; set; }
        public Guid UserId { get; set; }
        public List<string>? PhotoUrls { get; set; }
    }
}
