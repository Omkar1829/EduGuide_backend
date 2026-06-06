using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class UserCourse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid CourseId { get; set; }

    public string Status { get; set; } = null!;

    public int Progress { get; set; }

    public DateTime EnrolledAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
