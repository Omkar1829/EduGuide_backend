using System;
using System.Collections.Generic;

namespace EduGuide_Backend.Models;

public partial class KnowledgeArticle
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Industry { get; set; } = null!;

    public string? Url { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
