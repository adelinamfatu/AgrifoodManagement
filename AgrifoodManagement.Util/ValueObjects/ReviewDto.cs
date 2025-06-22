using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.ValueObjects
{
    public class ReviewDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
        public string ReviewerName { get; set; } = "";
        public DateTime Date { get; set; }
    }
}
