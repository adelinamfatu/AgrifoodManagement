using AgrifoodManagement.Util.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal OriginalPrice { get; set; }

        public decimal? CurrentPrice { get; set; }

        public double Quantity { get; set; }

        public MeasurementUnit? UnitOfMeasurement { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int? ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory? ProductCategory { get; set; }

        public AnnouncementStatus AnnouncementStatus { get; set; }

        public DateTime? TimePosted { get; set; }

        public bool IsPromoted { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User? Seller { get; set; }
    }
}
