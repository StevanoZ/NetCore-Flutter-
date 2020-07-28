using System.Collections.Generic;

namespace API.Dto
{
    public class ProductToReturnDto
    {
        public int Id { get; set; } 
        public  string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double NetPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureIUrl { get; set; }
        public string ProductBrand { get; set; }
        public string  ProductType { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<PhotoToReturnDto> Photos {get; set;}
    }
}