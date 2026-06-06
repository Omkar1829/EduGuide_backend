using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class UserJob
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid JobId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? AppliedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
