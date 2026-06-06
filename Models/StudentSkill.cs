using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class StudentSkill
{
    public Guid Id { get; set; }

    public Guid ProfileId { get; set; }

    public Guid SkillId { get; set; }

    public int Level { get; set; }

    public int? YearsExp { get; set; }

    public bool IsVerified { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual StudentProfile Profile { get; set; } = null!;

    public virtual Skill Skill { get; set; } = null!;
}
