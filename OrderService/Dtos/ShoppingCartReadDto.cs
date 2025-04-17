namespace OrderService.Dtos
{
    public class ShoppingCartReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ShoppingCartItemReadDto> Items { get; set; }
    }

    public class ShoppingCartItemReadDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
    }
}