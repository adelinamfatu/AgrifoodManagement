using AgrifoodManagement.Util;
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

        [Required]
        public DateTime? OrderedAt { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [ForeignKey("User")]
        public Guid BuyerId { get; set; }

        public virtual User? Buyer { get; set; }
    }
}
