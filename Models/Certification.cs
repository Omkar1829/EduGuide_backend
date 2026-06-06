using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class Certification
{
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; }

    public string Name { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? CredentialUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual StudentProfile Profile { get; set; } = null!;
}
