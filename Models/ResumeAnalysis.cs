using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class ResumeAnalysis
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string FileName { get; set; } = null!;

    public string? FileUrl { get; set; }

    public string? ParsedContent { get; set; }

    public string? Analysis { get; set; }

    public double? Score { get; set; }

    public string? Feedback { get; set; }

    public string? Recommendations { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
