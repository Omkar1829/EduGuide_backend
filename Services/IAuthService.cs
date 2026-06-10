using EduGuide_Backend.DTO.Auth;

namespace EduGuide_Backend.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto req);

        Task LogoutAsync(RefreshTokenRequestDto req);

        Task<UserDto> GetProfileAsync(Guid userId);

        Task UpdatePasswordAsync(Guid userId, UpdatePasswordRequestDto req);

        Task<UserDto> UpdateSubscriptionAsync(Guid userId, UpdateSubscriptionRequestDto req);
    }
}
