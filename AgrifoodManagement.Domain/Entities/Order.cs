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
    [Table("Orders")]
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? OrderedAt { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public double DeliveryLatitude { get; set; }

        [Required]
        public double DeliveryLongitude { get; set; }

        public string DeliveryMethod { get; set; } = "Normal";

        public decimal DeliveryFee { get; set; }

        public string PhoneNumber { get; set; }

        [ForeignKey("DiscountCode")]
        public string? DiscountCodeId { get; set; }

        public virtual DiscountCode? DiscountCode { get; set; }

        [ForeignKey("User")]
        public Guid BuyerId { get; set; }

        public virtual User? Buyer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
