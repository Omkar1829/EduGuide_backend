using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class ChatHistory
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string SessionId { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Metadata { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
