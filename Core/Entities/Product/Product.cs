using System.Collections.Generic;
using System.Linq;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double NetPrice { get; set; }
        public int Quantity { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public bool IsActive { get; set; }

        private readonly List<Photo> _photos = new List<Photo>();
        public IReadOnlyList<Photo> Photos => _photos.AsReadOnly();

        public void AddPhoto(string pictureUrl, string fileName, bool isMain = false)
        {
            var photo = new Photo
            {
                PictureUrl = pictureUrl,
                FileName = fileName,

            };
            if (_photos.Count == 0) photo.IsMain = true;
            _photos.Add(photo);
        }
        public void RemovePhoto(int id)
        {
            var photo = _photos.Find(p => p.Id == id);
            if (photo != null)
            {
                _photos.Remove(photo);
            }
        }

        public void SetMainPhoto(int id)
        {
            var currentMain = _photos.SingleOrDefault(p => p.IsMain);
            foreach (var item in _photos.Where(p => p.IsMain))
            {
                item.IsMain = false;
            }
            var photo = _photos.Find(p => p.Id == id);
            if (photo != null)
            {

                if (currentMain != null)
                {
                    photo.IsMain = true;
                    currentMain.IsMain = false;
                }
            }
            else
            {
                currentMain.IsMain = true;
            }
        }

    }
}