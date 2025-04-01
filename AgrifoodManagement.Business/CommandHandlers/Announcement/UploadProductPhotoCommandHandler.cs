using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util;
using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class UploadProductPhotoCommandHandler : IRequestHandler<UploadProductPhotoCommand, Result<Guid>>
    {
        private readonly IApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public UploadProductPhotoCommandHandler(IApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<Guid>> Handle(UploadProductPhotoCommand request, CancellationToken cancellationToken)
        {
            var photoUrls = new List<string>();

            foreach (var photo in request.Photos)
            {
                var url = await _cloudinaryService.UploadPhotoAsync(photo, PhotoFolder.Products);
                if (!string.IsNullOrEmpty(url))
                {
                    photoUrls.Add(url);
                }
            }

            // Save photos in the database

            return Result<Guid>.Success(new Guid());
        }
    }
}
