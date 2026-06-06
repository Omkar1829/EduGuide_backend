using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class Quiz
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Questions { get; set; } = null!;

    public string Status { get; set; } = null!;

    public double? TotalScore { get; set; }

    public double? MaxScore { get; set; }

    public int? Duration { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();

    public virtual User User { get; set; } = null!;
}
