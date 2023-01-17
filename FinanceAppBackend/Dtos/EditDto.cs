namespace FinanceAppBackend.Dtos
{
    public class EditDto
    {
        public string Name {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public string ProfileImage {get; set;}
        public bool DarkMode {get; set;}
        public bool Notifications {get; set;}
        public double Balance {get; set;}
    }
}