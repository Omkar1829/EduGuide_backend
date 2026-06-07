using EduGuide_Backend.DTO.Auth;

namespace EduGuide_Backend.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
