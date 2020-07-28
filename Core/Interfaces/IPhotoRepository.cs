using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interfaces
{
    public interface IPhotoRepository
    {
        Task<Photo> SaveToDiskAsync(IFormFile file);
        void DeleteFromDisk(Photo photo);
    }
}