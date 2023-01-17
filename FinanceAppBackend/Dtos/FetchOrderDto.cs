namespace FinanceAppBackend.Dtos
{
    public class FetchOrderDto
    {
        public int Id {get; set;}
        public string Type {get; set;}
        public string Date {get; set;}
        public int Quantity {get; set;}
        public double Price {get; set;}
        public int User {get; set;}
        public int Asset {get; set;}
        public string Symbol {get; set;}
        public string Name {get; set;}
        public double Lastsale {get; set;}
        public double Netchange {get; set;}
        public double Pctchange {get; set;}
        public double Marketcap {get; set;}
        
    }
}