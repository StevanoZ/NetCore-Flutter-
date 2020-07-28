using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Dto
{
    public class PhotoUploadDto
    {
        [MaxFileSize(3 * 1024 * 1024)]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile Photo { get; set; }
    }
}