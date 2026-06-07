using System;

namespace EduGuide_Backend.DTO.Auth;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string SubscriptionTier { get; set; } = string.Empty;
    public int ChatLimitRemaining { get; set; }
    public DateTime LastLimitReset { get; set; }
    public bool IsVerified { get; set; }
    public bool IsActive { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
