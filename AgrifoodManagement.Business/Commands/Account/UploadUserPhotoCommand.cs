using AgrifoodManagement.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Business.Commands.Account
{
    public class UploadUserPhotoCommand : IRequest<Result<Guid>>
    {
        public IFormFile Photo { get; set; }
        public int UserId { get; set; }
    }
}
