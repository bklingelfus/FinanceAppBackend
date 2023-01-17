namespace FinanceAppBackend.Dtos
{
    public class OrderDto
    {
        public string Type {get; set;}
        public int Quantity {get; set;}
        public double Price {get; set;}
        public int User {get; set;}
        public int Asset {get; set;}
    }
}