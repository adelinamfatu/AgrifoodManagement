using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.ValueObjects
{
    public class PromoteProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = "";
    }
}
