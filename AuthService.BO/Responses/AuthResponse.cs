
using System.ComponentModel.DataAnnotations;
using AuthService.BO.Enums;

namespace AuthService.BO.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;

        [EnumDataType(typeof(RoleName))]
        public RoleName RoleName { get; set; }
    }
}