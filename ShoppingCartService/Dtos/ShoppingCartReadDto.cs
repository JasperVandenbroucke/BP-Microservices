namespace ShoppingCartService.Dtos
{
    public class ShoppingCartReadDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ShoppingCartItemReadDto> Items { get; set; }
        public int Count => Items?.Count() ?? 0;
    }
}