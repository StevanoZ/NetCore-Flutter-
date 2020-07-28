using System.ComponentModel.DataAnnotations;

namespace API.Dto {
    public class ProductCreateDto {
        [Required]
        [MinLength (8, ErrorMessage = "Name at least must  be 8 characther")]
        [MaxLength (120, ErrorMessage = "Exceed limit characther, max 120 characther")]
        public string Name { get; set; }

        [Required]
        [MinLength (15, ErrorMessage = "Description at least must  be 15 characther")]
        [MaxLength (255, ErrorMessage = "Exceed limit characther, max 120 characther")]
        public string Description { get; set; }

        [RegularExpression ("^0*([1-8][0-9]{3}|9[0-8][0-9]{2}|99[0-8][0-9]|999[0-9]|[1-8][0-9]{4}|9[0-8][0-9]{3}|99[0-8][0-9]{2}|999[0-8][0-9]|9999[0-9]|[1-8][0-9]{5}|9[0-8][0-9]{4}|99[0-8][0-9]{3}|999[0-8][0-9]{2}|9999[0-8][0-9]|99999[0-9]|[1-8][0-9]{6}|9[0-8][0-9]{5}|99[0-8][0-9]{4}|999[0-8][0-9]{3}|9999[0-8][0-9]{2}|99999[0-8][0-9]|999999[0-9]|10000000)$", ErrorMessage = "Validation error price, minimum range greater than 999 and less than 10.000.001")]
        [Required]
        public double Price { get; set; }

        [Required]
        [RegularExpression ("^0*([1-8][0-9]{3}|9[0-8][0-9]{2}|99[0-8][0-9]|999[0-9]|[1-8][0-9]{4}|9[0-8][0-9]{3}|99[0-8][0-9]{2}|999[0-8][0-9]|9999[0-9]|[1-8][0-9]{5}|9[0-8][0-9]{4}|99[0-8][0-9]{3}|999[0-8][0-9]{2}|9999[0-8][0-9]|99999[0-9]|[1-8][0-9]{6}|9[0-8][0-9]{5}|99[0-8][0-9]{4}|999[0-8][0-9]{3}|9999[0-8][0-9]{2}|99999[0-8][0-9]|999999[0-9]|10000000)$", ErrorMessage = "Validation error min price, minimum range greater than 999 and less than 10.000.001")]
        public double NetPrice { get; set; }

        [Required]
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }

        [Required]
        public int ProductBrandId { get; set; }

        [Required]
        public int ProductTypeId { get; set; }
    }
}