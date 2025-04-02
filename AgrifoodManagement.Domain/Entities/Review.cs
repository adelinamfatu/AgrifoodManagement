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

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User Reviewer { get; set; }
    }
}
