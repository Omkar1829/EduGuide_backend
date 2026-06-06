using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class CareerRoadmap
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string TargetCareer { get; set; } = null!;

    public string Phases { get; set; } = null!;

    public int Progress { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
