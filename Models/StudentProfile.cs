using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class StudentProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? Bio { get; set; }

    public bool ProfileComplete { get; set; }

    public int CompletionPct { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AcademicRecord> AcademicRecords { get; set; } = new List<AcademicRecord>();

    public virtual ICollection<CareerGoal> CareerGoals { get; set; } = new List<CareerGoal>();

    public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();

    public virtual ICollection<Interest> Interests { get; set; } = new List<Interest>();

    public virtual ICollection<Strength> Strengths { get; set; } = new List<Strength>();

    public virtual ICollection<StudentSkill> StudentSkills { get; set; } = new List<StudentSkill>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Weakness> Weaknesses { get; set; } = new List<Weakness>();
}
