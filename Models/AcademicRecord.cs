using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class AcademicRecord
{
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; }

    public string Institution { get; set; } = null!;

    public string Degree { get; set; } = null!;

    public string FieldOfStudy { get; set; } = null!;

    public int StartYear { get; set; }

    public int? EndYear { get; set; }

    public double? Gpa { get; set; }

    public double? Percentage { get; set; }

    public bool IsCurrent { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual StudentProfile Profile { get; set; } = null!;

    public virtual ICollection<SubjectMark> SubjectMarks { get; set; } = new List<SubjectMark>();
}
