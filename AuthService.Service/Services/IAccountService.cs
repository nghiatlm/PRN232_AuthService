using AuthService.BO.Entities;
using AuthService.BO.Enums;
using AuthService.BO.Request;
using AuthService.BO.Responses;

namespace AuthService.Service.Services
{
    public interface IAccountService
    {
        public Task<Account?> GetByEmail(string email);
        public Task<AuthResponse?> Login(string email, string password);
        public Task<bool> Register(RegisterRequest request);
        public Task CreateProfile(int accountId, string firstName, string lastName);
        Task<bool> Add(string email, string password, RoleName roleName);
    }
}