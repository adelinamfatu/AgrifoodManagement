using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgrifoodManagement.Util.ValueObjects;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("ForumThreads")]
    public class ForumThread
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public ForumCategory Category { get; set; }

        public Guid CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<ForumPost> Posts { get; set; }
    }
}
