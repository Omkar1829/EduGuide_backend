using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class CareerGoal
{
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? TargetYear { get; set; }

    public int Priority { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual StudentProfile Profile { get; set; } = null!;
}
