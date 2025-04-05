using AgrifoodManagement.Business.Commands.Forum;
using AgrifoodManagement.Domain.Entities;
using AgrifoodManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Forum
{
    public class CreateForumThreadCommandHandler : IRequestHandler<CreateForumThreadCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public CreateForumThreadCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateForumThreadCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(u => u.Email == request.CreatedByUserEmail)
                .FirstOrDefault();

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var userId = user.Id;

            var forumThread = new ForumThread
            {
                Title = request.Title,
                Content = request.Content,
                Category = request.Category,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ForumThreads.Add(forumThread);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
