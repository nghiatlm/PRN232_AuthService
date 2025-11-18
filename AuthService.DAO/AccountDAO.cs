using AuthService.BO.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DAO
{
    public class AccountDAO
    {
        private AuthServiceDbContext _context;

        private static AccountDAO _instance;

        public static AccountDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AccountDAO();
                }
                return _instance;
            }
        }

        public AccountDAO()
        {
            _context = new AuthServiceDbContext();
        }

        public async Task<Account?> FindByEmail(string email) => await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

        public async Task<Account?> FindById(int id) => await _context.Accounts.FindAsync(id);

        public async Task<int> Add(Account account)
        {
            _context.Accounts.Add(account);
            return await _context.SaveChangesAsync();
        }

        public async Task<Account?> Login(string email, string password) => await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);

    }
}