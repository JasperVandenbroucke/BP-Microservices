using System.ComponentModel.DataAnnotations;

namespace ShoppingCartService.Dtos
{
    public class ShoppingCartItemCreateDto
    {
        [Required]
        public int ProductId { get; set; }
    }
}