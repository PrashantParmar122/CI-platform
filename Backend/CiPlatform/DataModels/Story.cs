using System;
using System.Collections.Generic;

namespace CiPlatform.DataModels;

public partial class Story
{
    public long StoryId { get; set; }

    public long UserId { get; set; }

    public long MissionId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int Status { get; set; }

    public DateTime? PublishedAt { get; set; }

    public long? ViewCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;

    public virtual ICollection<StoryMedium> StoryMedia { get; } = new List<StoryMedium>();

    public virtual User User { get; set; } = null!;
}
