using System.ComponentModel.DataAnnotations;

namespace API.Dto {
    public class BasketItemDto {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

       
    }
}