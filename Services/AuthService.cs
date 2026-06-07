using EduGuide_Backend.DTO.Auth;
using EduGuide_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace EduGuide_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly EgaidbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthService(EgaidbContext context, IPasswordService passwordService, IJwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Email or Password");
            }

            if (!_passwordService.Verify(req.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid Email or Password");
            }

            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role.ToString(),
                    SubscriptionTier = user.SubscriptionTier.ToString(),
                    ChatLimitRemaining = user.ChatLimitRemaining,
                    LastLimitReset = user.LastLimitReset,
                    IsVerified = user.IsVerified,
                    IsActive = user.IsActive,
                    AvatarUrl = user.AvatarUrl,
                    LastLoginAt = user.LastLoginAt,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                },
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }
    }
}
