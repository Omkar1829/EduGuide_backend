using EduGuide_Backend.Models;

namespace EduGuide_Backend.Services;

public interface IJwtService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}
