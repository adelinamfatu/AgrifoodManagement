using AgrifoodManagement.Util.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public MeasurementUnit? UnitOfMeasurement { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ViewCount { get; set; }
        public int InquiryCount { get; set; }
        public decimal EstimatedMarketPrice { get; set; }
        public bool IsPromoted { get; set; }
        public AnnouncementStatus AnnouncementStatus { get; set; }
        public List<string> PhotoUrls { get; set; }
    }
}
