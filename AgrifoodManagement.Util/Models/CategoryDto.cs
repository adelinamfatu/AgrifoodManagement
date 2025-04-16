using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<CategoryDto> Children { get; set; } = new();
    }
}
