using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class SubjectMark
{
    public Guid Id { get; set; }

    public Guid AcademicRecordId { get; set; }

    public string SubjectName { get; set; } = null!;

    public double Marks { get; set; }

    public double MaxMarks { get; set; }

    public string? Grade { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AcademicRecord AcademicRecord { get; set; } = null!;
}
