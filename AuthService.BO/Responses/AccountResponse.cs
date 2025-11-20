
using AuthService.BO.Enums;

namespace AuthService.BO.Responses
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public RoleName RoleName { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}