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
        public decimal OriginalPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public double Quantity { get; set; }
        public MeasurementUnit? UnitOfMeasurement { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? CartQuantity { get; set; }
        public int? WishlistQuantity { get; set; }
        public SentimentType? MajoritySentiment { get; set; }
        public double? SentimentConfidence { get; set; }
        public bool IsPromoted { get; set; }
        public int DiscountPercentage {  get; set; }
        public bool IsFavorited { get; set; }
        public double? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public string? ProducerName { get; set; }
        public AnnouncementStatus AnnouncementStatus { get; set; }
        public List<string> PhotoUrls { get; set; }
    }
}
