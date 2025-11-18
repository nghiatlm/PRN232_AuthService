
using AuthService.BO.Entities;
using AuthService.BO.Enums;

namespace AuthService.Repository
{
    public interface IAccountRepository
    {
        Task<Account> FindByEmail(string email);
        Task<Account> Login(string email, string password);
        Task<bool> Register(Account account);
        Task<int> Add(string email, string password, RoleName role);

    }
}