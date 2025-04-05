using AgrifoodManagement.Util.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Queries.Forum
{
    public class GetForumTopicsQuery : IRequest<List<ForumTopicDto>> { }
}
