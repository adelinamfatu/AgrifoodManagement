using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public Guid Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [Required]
        public Guid ReviewerId { get; set; }

        [ForeignKey(nameof(ReviewerId))]
        [InverseProperty(nameof(User.ReviewsWritten))]
        public User Reviewer { get; set; }
    }
}
