using System;
using System.IO;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        public void DeleteFromDisk(Photo photo)
        {
            if (File.Exists(Path.Combine("Content/Images/Products", photo.FileName)))
            {
                File.Delete("Content/Images/Products/" + photo.FileName);
            }
        }

        public async Task<Photo> SaveToDiskAsync(IFormFile file)
        {
            var photo = new Photo();
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine("Content/Images/Products", fileName);
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);
                photo.FileName = fileName;
                photo.PictureUrl = "Images/Products/" + fileName;
                return photo;
            }
            return null;
        }
    }
}