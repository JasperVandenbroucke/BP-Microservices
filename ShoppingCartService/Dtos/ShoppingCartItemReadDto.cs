namespace ShoppingCartService.Dtos
{
    public class ShoppingCartItemReadDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
    }
}