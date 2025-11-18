
using AuthService.BO.Entities;
using AuthService.BO.Enums;
using AuthService.DAO;

namespace AuthService.Repository
{
    public class AccountRepository : IAccountRepository
    {
        public Task<int> Add(string email, string password, RoleName role)
        {
            var account = new Account
            {
                Email = email,
                Password = password,
                RoleName = role
            };
            return AccountDAO.Instance.Add(account);
        }

        public Task<Account?> FindByEmail(string email) => AccountDAO.Instance.FindByEmail(email) ?? throw new Exception("Account not found");

        public async Task<Account?> Login(string email, string password)
        {
            var account = await AccountDAO.Instance.Login(email, password);
            return account;
        }

        public async Task<bool> Register(Account account)
        {
            return await AccountDAO.Instance.Add(account) > 0 ? true : false;
        }
    }
}