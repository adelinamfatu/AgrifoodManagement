using AgrifoodManagement.Business.Queries.Forum;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AgrifoodManagement.Business.CommandHandlers.Forum
{
    public class GetForumTopicsQueryHandler : IRequestHandler<GetForumTopicsQuery, List<ForumTopicDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetForumTopicsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ForumTopicDto>> Handle(GetForumTopicsQuery request, CancellationToken cancellationToken)
        {
            var topics = await _context.ForumThreads
                .Include(t => t.Author)
                .Include(t => t.Posts)
                    .ThenInclude(p => p.User)
                .ToListAsync(cancellationToken);

            return topics.Select(t => new ForumTopicDto
            {
                Title = t.Title,
                Text = t.Content,
                Category = t.Category.ToString(),
                Author = MapUser(t.Author),
                LatestReplyAuthor = t.Posts
                    .OrderByDescending(p => p.PostedAt)
                    .FirstOrDefault()?.User?.FirstName + " " + t.Posts.OrderByDescending(p => p.PostedAt).FirstOrDefault()?.User?.LastName ?? "N/A",
                LatestReplyTime = t.Posts
                    .OrderByDescending(p => p.PostedAt)
                    .FirstOrDefault()?.PostedAt,
                CommentsCount = t.Posts.Count,
                TopCommenters = t.Posts
                    .GroupBy(p => p.User)
                    .OrderByDescending(g => g.Count())
                    .Take(4)
                    .Select(g => MapUser(g.Key))
                    .ToList(),
                Comments = t.Posts.Select(p => new ForumCommentDto
                {
                    Author = MapUser(p.User),
                    Text = p.Message,
                    CreatedAt = p.PostedAt
                }).ToList()
            }).ToList();
        }

        private ForumUserDto MapUser(User user)
        {
            return new ForumUserDto
            {
                Name = user.FirstName + user.LastName,
                AvatarUrl = user.Avatar ?? "/images/avatar-placeholder.png"
            };
        }
    }
}
