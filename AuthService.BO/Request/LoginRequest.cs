
using System.ComponentModel.DataAnnotations;

namespace AuthService.BO.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(16, ErrorMessage = "Password cannot exceed 16 characters")]
        public string Password { get; set; } = string.Empty;
    }
}