namespace OrderService.Dtos
{
    public class OrderItemReadDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}