using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public bool IsVerified { get; set; }

    public bool IsActive { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ChatLimitRemaining { get; set; }

    public DateTime LastLimitReset { get; set; }

    public virtual ICollection<CareerRoadmap> CareerRoadmaps { get; set; } = new List<CareerRoadmap>();

    public virtual ICollection<ChatHistory> ChatHistories { get; set; } = new List<ChatHistory>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<ResumeAnalysis> ResumeAnalyses { get; set; } = new List<ResumeAnalysis>();

    public virtual StudentProfile? StudentProfile { get; set; }

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();

    public virtual ICollection<UserJob> UserJobs { get; set; } = new List<UserJob>();
}
