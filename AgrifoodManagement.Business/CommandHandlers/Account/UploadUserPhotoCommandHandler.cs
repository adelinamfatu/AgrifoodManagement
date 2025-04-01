using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Account
{
    public class UploadUserPhotoCommandHandler : IRequestHandler<UploadUserPhotoCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public UploadUserPhotoCommandHandler(IApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<string>> Handle(UploadUserPhotoCommand request, CancellationToken cancellationToken)
        {
            var imageUrl = await _cloudinaryService.UploadPhotoAsync(request.Photo, request.PhotoFolder);
            if (string.IsNullOrEmpty(imageUrl))
            {
                return Result<string>.Failure("Upload failed.");
            }

            var user = await _context.Users.Where(x => x.Email == request.UserEmail)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.Avatar = imageUrl;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Result<string>.Success(imageUrl);
        }
    }
}
