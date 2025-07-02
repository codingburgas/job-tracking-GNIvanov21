using JobTracking.Application.DTOs;
using JobTracking.DataAccess;
using JobTracking.Domain.Enums;
using JobTracking.Domain.Models;
using System.Security.Cryptography;
using System.Text;

namespace JobTracking.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDataService _dataService;

        public AuthService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public User? Register(RegisterUserDto registerDto)
        {
            var db = _dataService.GetDatabase();

            if (db.Users.Any(u => u.Username == registerDto.Username))
            {
                return null; // Username already exists
            }

            var newUser = new User
            {
                Id = db.Users.Any() ? db.Users.Max(u => u.Id) + 1 : 1,
                FirstName = registerDto.FirstName,
                MiddleName = registerDto.MiddleName,
                LastName = registerDto.LastName,
                Username = registerDto.Username,
                PasswordHash = HashPassword(registerDto.Password),
                Role = Role.User
            };

            db.Users.Add(newUser);
            _dataService.SaveChanges(db);

            return newUser;
        }

        public LoginResponseDto? Login(LoginRequestDto loginDto)
        {
            var db = _dataService.GetDatabase();
            var user = db.Users.FirstOrDefault(u => u.Username == loginDto.Username);

            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            // In a real app, you would generate a real JWT token here.
            // For our simple case, we'll create a placeholder token.
            var token = $"simple-token-for-user-{user.Id}";

            return new LoginResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}