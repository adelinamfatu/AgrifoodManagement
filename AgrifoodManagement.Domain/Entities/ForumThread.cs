using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Domain.Entities
{
    [Table("ForumThreads")]
    public class ForumThread
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<ForumPost> Posts { get; set; }
    }
}
