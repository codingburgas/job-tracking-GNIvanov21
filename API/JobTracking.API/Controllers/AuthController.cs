using JobTracking.Application.DTOs;
using JobTracking.Application.Services;
using JobTracking.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDataService _dataService; // Needed for GetAllUsers

        public AuthController(IAuthService authService, IDataService dataService)
        {
            _authService = authService;
            _dataService = dataService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto registerDto)
        {
            var user = _authService.Register(registerDto);
            if (user == null)
            {
                return BadRequest("Username already exists.");
            }
            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto loginDto)
        {
            var loginResponse = _authService.Login(loginDto);
            if (loginResponse == null)
            {
                return Unauthorized("Invalid credentials.");
            }
            return Ok(loginResponse);
        }

        // This endpoint was added to support the admin mailbox feature
        [HttpGet("/api/users")]
        public IActionResult GetAllUsers()
        {
            var db = _dataService.GetDatabase();
            return Ok(db.Users);
        }
    }
}