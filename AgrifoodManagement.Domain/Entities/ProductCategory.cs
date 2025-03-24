using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("ProductCategories")]
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("ParentCategoryId")]
        public int? ParentCategoryId { get; set; }

        public ProductCategory? ParentCategory { get; set; }

        public ICollection<ProductCategory>? SubCategories { get; set; }
    }
}
