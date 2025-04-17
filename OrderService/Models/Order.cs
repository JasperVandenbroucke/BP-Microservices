using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }

        [Required]
        public int UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new();

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public string Status { get; set; }
    }
}