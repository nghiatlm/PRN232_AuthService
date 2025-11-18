using AuthService.BO.Request;
using AuthService.BO.Responses;
using AuthService.Service.Services;
using Microsoft.AspNetCore.Mvc;
namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            var result = await _accountService.Add(request.Email, request.Password, request.RoleName);
            return result ? Ok(ApiResponse<object>.SuccessResponse(null, "Create successful")) : BadRequest(ApiResponse<bool>.BadRequest("Account creation failed"));
        }
    }
}