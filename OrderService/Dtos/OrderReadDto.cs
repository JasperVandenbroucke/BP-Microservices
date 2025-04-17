namespace OrderService.Dtos
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int UserId { get; set; }
        public List<OrderItemReadDto> Items { get; set; } = new();
        public double TotalPrice { get; set; }
        public string Status { get; set; }
    }
}