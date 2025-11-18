
using System.Security.Claims;
using AuthService.BO.Entities;

namespace AuthService.Service.Services
{
    public interface IJwtService
    {
        public string GenerateJwtToken(Account _account);
        public int? ValidateToken(string token);
        public ClaimsPrincipal ValidateTokenClaimsPrincipal(string token);
        public string RefeshToken(string email);
    }
}