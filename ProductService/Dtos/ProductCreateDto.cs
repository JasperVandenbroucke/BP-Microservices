using System.ComponentModel.DataAnnotations;

namespace ProductService.Dtos
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }
    }
}