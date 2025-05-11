using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class QuarterlySalesDto
    {
        public string Period { get; init; } = null!;
        public double Sales { get; init; }
    }
}
