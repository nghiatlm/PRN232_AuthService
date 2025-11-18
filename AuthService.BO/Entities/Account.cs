
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AuthService.BO.Enums;
namespace AuthService.BO.Entities
{
    [Table("account")]
    public class Account
    {
        [Key]
        [Column("account_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Column("email", TypeName = "varchar(300)")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Column("password", TypeName = "varchar(300)")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(16, ErrorMessage = "Password cannot exceed 16 characters")]
        public string Password { get; set; } = string.Empty;

        [Column("role_name", TypeName = "varchar(100)")]
        [EnumDataType(typeof(RoleName), ErrorMessage = "Invalid role name")]
        public RoleName RoleName { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}