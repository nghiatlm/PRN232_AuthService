using AuthService.BO.Request;
using AuthService.BO.Responses;
using AuthService.Service.Services;
using Microsoft.AspNetCore.Mvc;
namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var account = await _accountService.Login(request.Email, request.Password);
            return account != null ? Ok(ApiResponse<AuthResponse>.SuccessResponse(account, "Login successful")) : Unauthorized(ApiResponse<object>.Unauthorized("Invalid email or password"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var account = await _accountService.Register(request);
            return account ? Ok(ApiResponse<AuthResponse>.SuccessResponse(null, "Registration successful")) : BadRequest(ApiResponse<object>.BadRequest("Registration failed"));
        }
    }
}