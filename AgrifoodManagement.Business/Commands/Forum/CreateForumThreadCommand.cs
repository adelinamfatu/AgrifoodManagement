using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Forum
{
    public class CreateForumThreadCommand : IRequest<bool>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public ForumCategory Category { get; set; }
        public string CreatedByUserEmail { get; set; }
    }
}
