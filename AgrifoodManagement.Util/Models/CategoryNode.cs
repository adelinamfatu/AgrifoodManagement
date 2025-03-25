using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class CategoryNode
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int? ParentId { get; set; }
        public bool HasChildren { get; set; }
    }
}
