namespace ShoppingCartService.Dtos
{
    public class OrderPublishedDto
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int UserId { get; set; }
        public string Event { get; set; }
    }
}