using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class Recommendation
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Confidence { get; set; }

    public string Reasoning { get; set; } = null!;

    public string? Metadata { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
