namespace EduGuide_Backend.DTO.Auth
{
    public class UpdatePasswordRequestDto
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
