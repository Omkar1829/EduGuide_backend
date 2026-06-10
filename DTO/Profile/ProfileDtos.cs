using System;
using System.Collections.Generic;

namespace EduGuide_Backend.DTO.Profile
{
    public class StudentProfileDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
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

        public List<AcademicRecordDto> AcademicRecords { get; set; } = new();
        public List<InterestDto> Interests { get; set; } = new();
        public List<CareerGoalDto> CareerGoals { get; set; } = new();
        public List<StrengthDto> Strengths { get; set; } = new();
        public List<WeaknessDto> Weaknesses { get; set; } = new();
        public List<StudentSkillDto> Skills { get; set; } = new();
        public List<CertificationDto> Certifications { get; set; } = new();
    }

    public class UpdateProfileRequestDto
    {
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Bio { get; set; }
    }

    public class AcademicRecordDto
    {
        public Guid Id { get; set; }
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public double? Gpa { get; set; }
        public double? Percentage { get; set; }
        public bool IsCurrent { get; set; }
        public List<SubjectMarkDto> SubjectMarks { get; set; } = new();
    }

    public class AcademicRecordRequestDto
    {
        public string Institution { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string FieldOfStudy { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public double? Gpa { get; set; }
        public double? Percentage { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class SubjectMarkDto
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public double Marks { get; set; }
        public double MaxMarks { get; set; }
        public string? Grade { get; set; }
    }

    public class SubjectMarkRequestDto
    {
        public string SubjectName { get; set; } = string.Empty;
        public double Marks { get; set; }
        public double MaxMarks { get; set; }
        public string? Grade { get; set; }
    }

    public class InterestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Level { get; set; }
    }

    public class InterestRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Level { get; set; }
    }

    public class CareerGoalDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? TargetYear { get; set; }
        public int Priority { get; set; }
    }

    public class CareerGoalRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? TargetYear { get; set; }
        public int Priority { get; set; }
    }

    public class StrengthDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Evidence { get; set; }
    }

    public class WeaknessDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Evidence { get; set; }
    }

    public class StrengthWeaknessRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Evidence { get; set; }
    }

    public class StudentSkillDto
    {
        public Guid Id { get; set; }
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Level { get; set; }
        public int? YearsExp { get; set; }
        public bool IsVerified { get; set; }
    }

    public class StudentSkillRequestDto
    {
        public string SkillName { get; set; } = string.Empty; // Create skill if not exists, map to it
        public int Level { get; set; }
        public int? YearsExp { get; set; }
    }

    public class CertificationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CredentialUrl { get; set; }
    }

    public class CertificationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? CredentialUrl { get; set; }
    }

    public class SkillSearchResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
