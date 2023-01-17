using FinanceAppBackend.Models;

namespace FinanceAppBackend.Contexts
{
    public interface IUserRepository
    {
        User Create(User user);
        User Update(User user);
        User Delete(User user);
        User GetByEmail(string email);
        User GetById(int id);
        Order Execute(Order order);
    }
}