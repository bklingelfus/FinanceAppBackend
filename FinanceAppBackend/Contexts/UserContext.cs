using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceAppBackend.Models;

namespace FinanceAppBackend.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<User>(entity => {entity.HasIndex(e => e.Email).IsUnique();});
            modelBuilder.Entity<Stock>(entity => {entity.HasIndex(e => e.Symbol).IsUnique();});
        }
    }
}