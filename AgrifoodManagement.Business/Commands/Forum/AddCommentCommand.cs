using AgrifoodManagement.Util.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Forum
{
    public class AddCommentCommand : IRequest<CommentResultDto>
    {
        public Guid TopicId { get; set; }
        public string CommentText { get; set; }
        public string CreatedByUserEmail { get; set; }
    }
}
