using FinanceAppBackend.Models;
using FinanceAppBackend.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using Npgsql;

namespace FinanceAppBackend.Contexts
{
    public class UserRepository: IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            user.Id = _context.SaveChanges();

            return user;
        }
        public Order Execute(Order order)
        {
            _context.Orders.Add(order);
            order.Id = _context.SaveChanges();

            return order;
        }
        public User Update(User user)
        {
            var editedUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            
            editedUser.Name = user.Name;
            editedUser.Email = user.Email;
            editedUser.ProfileImage = user.ProfileImage;
            editedUser.DarkMode = user.DarkMode;
            editedUser.Notifications = user.Notifications;
            editedUser.Balance = user.Balance;

            _context.SaveChanges();

            return editedUser;
        }
        public User Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();

            return user;
        }
        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}