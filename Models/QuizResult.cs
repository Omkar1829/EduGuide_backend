using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class QuizResult
{
    public Guid Id { get; set; }

    public Guid QuizId { get; set; }

    public Guid UserId { get; set; }

    public double Score { get; set; }

    public double MaxScore { get; set; }

    public double Percentage { get; set; }

    public string Answers { get; set; } = null!;

    public string? Analysis { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
