using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("ForumPosts")]
    public class ForumPost
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ThreadId { get; set; }

        [ForeignKey("ThreadId")]
        public ForumThread Thread { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public string Message { get; set; }

        public DateTime PostedAt { get; set; }
    }
}
