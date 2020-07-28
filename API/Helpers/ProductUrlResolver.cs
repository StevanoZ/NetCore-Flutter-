using System.Linq;
using API.Dto;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            var photo = source.Photos.FirstOrDefault(p => p.IsMain);
            if(photo != null){
                return _config["ApiUrl"] + photo.PictureUrl;
            }
            return _config["ApiUrl"] + "Images/Products/placeholder.png";
        }
    }
}