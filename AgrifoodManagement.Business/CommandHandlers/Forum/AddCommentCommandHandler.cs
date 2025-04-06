using AgrifoodManagement.Business.Commands.Forum;
using AgrifoodManagement.Domain;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Util.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Forum
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentResultDto>
    {
        private readonly ApplicationDbContext _context;

        public AddCommentCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommentResultDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.CreatedByUserEmail, cancellationToken);
            if (user == null)
            {
                return new CommentResultDto { Success = false };
            }

            var forumThread = await _context.ForumThreads
                .FirstOrDefaultAsync(t => t.Id == request.TopicId, cancellationToken);
            if (forumThread == null)
            {
                return new CommentResultDto { Success = false };
            }

            var comment = new ForumPost
            {
                Id = Guid.NewGuid(),
                ThreadId = request.TopicId,
                UserId = user.Id,
                Message = request.CommentText,
                PostedAt = DateTime.UtcNow
            };

            _context.ForumPosts.Add(comment);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return new CommentResultDto
                {
                    Success = true,
                    AuthorName = $"{user.FirstName} {user.LastName}",
                    AvatarUrl = user.Avatar,
                    TimeAgo = "Just now"
                };
            }
            catch (Exception)
            {
                return new CommentResultDto { Success = false };
            }
        }
    }
}
