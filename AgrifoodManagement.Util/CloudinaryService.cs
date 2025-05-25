using AgrifoodManagement.Util.ValueObjects;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AgrifoodManagement.Util
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                "dr3iuxco5",
                "498983293693911",
                "2jbp60CvfvywL0MIoPFmQ5ax_r4");

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<string?> UploadPhotoAsync(IFormFile photo, PhotoFolder folder)
        {
            if (photo == null || photo.Length == 0)
                return null;

            using (var stream = photo.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName, stream),
                    Folder = folder.ToString().ToLower()
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
        }

        public async Task<bool> DeletePhotoAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            // Extract public ID by trimming domain and file extension
            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Trim('/').Split('/');
            var publicId = Path.ChangeExtension(string.Join('/', segments), null);

            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }

        public async Task<string?> UploadRawAsync(Stream stream, string fileName, string folder)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = folder.ToLowerInvariant(),
                ResourceType = ResourceType.Raw
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl?.ToString();
        }
    }
}
