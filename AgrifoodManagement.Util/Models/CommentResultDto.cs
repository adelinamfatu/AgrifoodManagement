using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class CommentResultDto
    {
        public bool Success { get; set; }
        public string AuthorName { get; set; }
        public string AvatarUrl { get; set; }
        public string TimeAgo { get; set; }
    }
}
