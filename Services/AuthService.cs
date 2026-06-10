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

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto req)
        {
            if (await _context.Users.AnyAsync(u => u.Email == req.email))
            {
                throw new InvalidOperationException("Email already in use");
            }
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = req.email,
                FirstName = req.firstName,
                LastName = req.lastname,
                PasswordHash = _passwordService.Hash(req.password),
                Role = UserRole.STUDENT,
                SubscriptionTier = SubscriptionTier.NEWBIE,
                ChatLimitRemaining = 100,
                LastLimitReset = DateTime.UtcNow,
                IsVerified = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
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
            return new RegisterResponseDto
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
                Accesstoken = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto req)
        {
            var tokenRecord = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == req.RefreshToken);

            if (tokenRecord == null || tokenRecord.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var user = tokenRecord.User;

            _context.RefreshTokens.Remove(tokenRecord);

            var newAccessToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newRefreshToken,
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
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task LogoutAsync(RefreshTokenRequestDto req)
        {
            var tokenRecord = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == req.RefreshToken);

            if (tokenRecord != null)
            {
                _context.RefreshTokens.Remove(tokenRecord);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserDto> GetProfileAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return new UserDto
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
            };
        }

        public async Task UpdatePasswordAsync(Guid userId, UpdatePasswordRequestDto req)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (!_passwordService.Verify(req.OldPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Incorrect old password");
            }

            user.PasswordHash = _passwordService.Hash(req.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> UpdateSubscriptionAsync(Guid userId, UpdateSubscriptionRequestDto req)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (!Enum.TryParse<SubscriptionTier>(req.SubscriptionTier, true, out var parsedTier))
            {
                throw new ArgumentException("Invalid subscription tier. Allowed: NEWBIE, PRO, PRO_PLUS");
            }

            user.SubscriptionTier = parsedTier;

            user.ChatLimitRemaining = parsedTier switch
            {
                SubscriptionTier.NEWBIE => 5,
                SubscriptionTier.PRO => 20,
                SubscriptionTier.PRO_PLUS => 50,
                _ => 5
            };

            user.LastLimitReset = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UserDto
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
            };
        }
    }
}
