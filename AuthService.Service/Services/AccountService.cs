
using System.Net;
using System.Net.Http.Json;
using AuthService.BO.Entities;
using AuthService.BO.Enums;
using AuthService.BO.Exceptions;
using AuthService.BO.Request;
using AuthService.BO.Responses;
using AuthService.Repository;
using Microsoft.Extensions.Logging;

namespace AuthService.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly HttpClient _http;


        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger, IPasswordHasher passwordHasher, IJwtService jwtService, HttpClient http)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _http = http;
        }

        public async Task CreateProfile(int accountId, string firstName, string lastName)
        {
            try
            {
                await _http.PostAsJsonAsync("/api/v1/public/register", new
                {
                    accountId = accountId,
                    firstName = firstName,
                    lastName = lastName
                });
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Account?> GetByEmail(string email)
        {
            try
            {
                var account = await _accountRepository.FindByEmail(email);
                return account ?? throw new AppException("Account not found", HttpStatusCode.NotFound);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<AuthResponse?> Login(string email, string password)
        {
            try
            {
                var account = await _accountRepository.FindByEmail(email);
                if (account == null) throw new AppException("Account does not exist", HttpStatusCode.Unauthorized);
                bool isPasswordValid = _passwordHasher.VerifyPassword(password, account.Password);
                if (!isPasswordValid) throw new AppException("Invalid email or password", HttpStatusCode.BadRequest);
                string token = _jwtService.GenerateJwtToken(account);
                if (token == null) throw new AppException("Token cannot be empty", HttpStatusCode.Unauthorized);
                var authResponse = new AuthResponse
                {
                    Token = token,
                    RoleName = account.RoleName
                };
                return authResponse ?? throw new AppException("Invalid email or password", HttpStatusCode.Unauthorized);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            try
            {
                var accountExisting = await _accountRepository.FindByEmail(request.Email);
                if (accountExisting != null)
                {
                    throw new AppException("Email is already registered", HttpStatusCode.Conflict);
                }
                accountExisting = new Account
                {
                    Email = request.Email,
                    Password = _passwordHasher.HashPassword(request.Password),
                    RoleName = RoleName.ROLE_CUSTOMER
                };
                var result = await _accountRepository.Register(accountExisting);
                await CreateProfile(accountExisting.Id, request.FirstName, request.LastName);
                return result ? true : throw new AppException("Register failed", HttpStatusCode.BadRequest);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<bool> Add(string email, string password, RoleName roleName)
        {
            try
            {
                var accountExisting = await _accountRepository.FindByEmail(email);
                if (accountExisting != null)
                {
                    throw new AppException("Email is already registered", HttpStatusCode.Conflict);
                }
                accountExisting = new Account
                {
                    Email = email,
                    Password = _passwordHasher.HashPassword(password),
                    RoleName = roleName
                };
                var result = await _accountRepository.Register(accountExisting);
                return result ? true : throw new AppException("Register failed", HttpStatusCode.BadRequest);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}