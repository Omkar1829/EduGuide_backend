using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class Strength
{
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; }

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public string? Evidence { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual StudentProfile Profile { get; set; } = null!;
}
