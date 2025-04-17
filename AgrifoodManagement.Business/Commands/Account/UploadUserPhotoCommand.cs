using AgrifoodManagement.Util.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Account
{
    public class UploadUserPhotoCommand : IRequest<Result<string>>
    {
        public IFormFile Photo { get; set; }
        public string UserEmail { get; set; }
        public PhotoFolder PhotoFolder { get; set; }
    }
}
