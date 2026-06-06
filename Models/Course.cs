using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class Course
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Provider { get; set; } = null!;

    public string? Url { get; set; }

    public string? Duration { get; set; }

    public string? Level { get; set; }

    public string Category { get; set; } = null!;

    public double? Price { get; set; }

    public string? Currency { get; set; }

    public double? Rating { get; set; }

    public int EnrolledCount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
