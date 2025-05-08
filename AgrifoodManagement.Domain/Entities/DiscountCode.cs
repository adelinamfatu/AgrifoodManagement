using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgrifoodManagement.Util.ValueObjects;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("DiscountCodes")]
    public class DiscountCode
    {
        [Key]
        public string Code { get; set; }

        [Required]
        public DiscountType Type { get; set; }

        [Required]
        public decimal Value { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public string? Description { get; set; }
    }
}
