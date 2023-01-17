using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAppBackend.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id {get; set;}
        [Column("name")]
        public string Name {get; set;}
        [Column("email")]
        public string Email {get; set;}
        [Column("password")]
        public string Password {get; set;}
        [Column("profileImage")]
        public string ProfileImage {get; set;}
        [Column("darkMode")]
        public bool DarkMode {get; set;}
        [Column("notifications")]
        public bool Notifications {get; set;}
        [Column("balance")]
        public double Balance {get; set;}
        // public virtual ICollection<Order> Orders { get; set; }

        public User ()
        {
            this.ProfileImage = "Image";
            this.DarkMode = false;
            this.Notifications = false;
            this.Balance = 0;
        }
    }

    [Table("stocks")]
    public class Stock
    {
        [Column("id")]
        public int Id {get; set;}
        [Column("symbol")]
        public string Symbol {get; set;}
        [Column("name")]
        public string Name {get; set;}
        [Column("lastsale")]
        public double Lastsale {get; set;}
        [Column("netchange")]
        public double Netchange {get; set;}
        [Column("pctchange")]
        public double Pctchange {get; set;}
        [Column("marketcap")]
        public double Marketcap {get; set;}
        // public virtual ICollection<Order> Orders { get; set; }
    }

    [Table("orders")]
    public class Order
    {
        [Column("id")]
        public int Id {get; set;}
        [Column("type")]
        public string Type {get; set;}
        [Column("date")]
        public string Date {get; set;}
        [Column("quantity")]
        public int Quantity {get; set;}
        [Column("price")]
        public double Price {get; set;}
        [Column("user")]
        public int User {get; set;}
        [Column("asset")]
        public int Asset {get; set;}

        public Order ()
        {
            this.Date = DateTime.Now.ToShortDateString();
        }
    }

}