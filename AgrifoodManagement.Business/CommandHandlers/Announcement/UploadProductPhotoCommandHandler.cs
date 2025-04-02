using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain.Entities;
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
    public class UploadProductPhotoCommandHandler : IRequestHandler<UploadProductPhotoCommand, Result<string>>
    {
        private readonly IApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public UploadProductPhotoCommandHandler(IApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<string>> Handle(UploadProductPhotoCommand request, CancellationToken cancellationToken)
        {
            foreach (var photo in request.Photos)
            {
                var url = await _cloudinaryService.UploadPhotoAsync(photo, PhotoFolder.Products);
                if (!string.IsNullOrEmpty(url))
                {
                    var extendedProperty = new ExtendedProperty
                    {
                        ID = Guid.NewGuid(),
                        EntityType = "Product",
                        EntityId = request.ProductId,
                        Key = "PhotoUrl",
                        Value = url
                    };

                    _context.ExtendedProperties.Add(extendedProperty);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Photos uploaded successfully.");
        }
    }
}
