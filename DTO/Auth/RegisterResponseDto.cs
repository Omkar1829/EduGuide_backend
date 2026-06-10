namespace EduGuide_Backend.DTO.Auth
{
    public class RegisterResponseDto
    {
        public UserDto User { get; set; } = null!;
        public string Accesstoken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
