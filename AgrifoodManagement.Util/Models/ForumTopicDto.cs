using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util.Models
{
    public class ForumTopicDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }

        public ForumUserDto Author { get; set; }

        public string LatestReplyAuthor { get; set; }
        public DateTime? LatestReplyTime { get; set; }

        public int CommentsCount { get; set; }
        public List<ForumUserDto> TopCommenters { get; set; } = new();
        public List<ForumCommentDto> Comments { get; set; } = new();
    }
}
