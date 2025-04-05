using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class ForumCommentDto
    {
        public ForumUserDto Author { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
