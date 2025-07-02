using JobTracking.Application.DTOs;
using JobTracking.Domain.Models;

namespace JobTracking.Application.Services
{
    public interface IAuthService
    {
        User? Register(RegisterUserDto registerDto);
        LoginResponseDto? Login(LoginRequestDto loginDto);
    }
}