using AgrifoodManagement.Business.Commands.Product;
using AgrifoodManagement.Domain.Interfaces;
using AgrifoodManagement.Util;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.CommandHandlers.Announcement
{
    public class DeleteProductPhotosCommandHandler : IRequestHandler<DeleteProductPhotosCommand, Unit>
    {
        private readonly CloudinaryService _cloudinary;
        private readonly IApplicationDbContext _context;

        public DeleteProductPhotosCommandHandler(CloudinaryService cloudinary, IApplicationDbContext context)
        {
            _cloudinary = cloudinary;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductPhotosCommand request, CancellationToken cancellationToken)
        {
            var props = _context.ExtendedProperties
                .Where(ep => ep.EntityType == "Product"
                          && ep.EntityId == request.ProductId
                          && ep.Key == "PhotoUrl")
                .ToList();

            foreach (var prop in props)
            {
                // delete from Cloudinary by URL or public ID
                await _cloudinary.DeletePhotoAsync(prop.Value);
            }

            // remove from database
            _context.ExtendedProperties.RemoveRange(props);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
