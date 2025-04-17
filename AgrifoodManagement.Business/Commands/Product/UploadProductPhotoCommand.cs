using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Product
{
    public class UploadProductPhotoCommand : IRequest<Result<string>>
    {
        public List<IFormFile> Photos { get; set; }
        public Guid ProductId { get; set; }
        public PhotoFolder Folder { get; set; }
    }
}
