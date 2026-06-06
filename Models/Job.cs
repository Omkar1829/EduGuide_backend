using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class Job
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Company { get; set; } = null!;

    public string? Location { get; set; }

    public string? Url { get; set; }

    public string? SalaryRange { get; set; }

    public string? Experience { get; set; }

    public List<string>? Skills { get; set; }

    public string Category { get; set; } = null!;

    public string? Type { get; set; }

    public bool IsActive { get; set; }

    public DateTime PostedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<UserJob> UserJobs { get; set; } = new List<UserJob>();
}
