using AgrifoodManagement.Business.Commands.Account;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Account
{
    public class UploadUserPhotoCommandHandler : IRequestHandler<UploadUserPhotoCommand, Result<Guid>>
    {
        private readonly IApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public UploadUserPhotoCommandHandler(IApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<Guid>> Handle(UploadUserPhotoCommand request, CancellationToken cancellationToken)
        {
            var imageUrl = await _cloudinaryService.UploadPhotoAsync(request.Photo, request.PhotoFolder);
            if (string.IsNullOrEmpty(imageUrl))
            {
                return Result<Guid>.Failure("Upload final.");
            }

            //var user = await _context.Users.FindAsync(request.UserId);
            //if (user != null)
            //{
            //    user.AvatarUrl = imageUrl;
            //    await _context.SaveChangesAsync(cancellationToken);
            //}

            //return Result<Guid>.Success(user.Id);

            return Result<Guid>.Success(new Guid());
        }
    }
}
