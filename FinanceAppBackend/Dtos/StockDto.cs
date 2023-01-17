namespace FinanceAppBackend.Dtos
{
    public class StockDto
    {
        public string Symbol {get; set;}
        public string Name {get; set;}
        public string Lastsale {get; set;}
        public string Netchange {get; set;}
        public string Pctchange {get; set;}
        public string Marketcap {get; set;}
    }
}