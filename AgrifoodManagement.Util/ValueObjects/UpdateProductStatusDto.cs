using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.ValueObjects
{
    public class UpdateProductStatusDto
    {
        public Guid ProductId { get; set; }
        public AnnouncementStatus NewStatus { get; set; }
    }
}
