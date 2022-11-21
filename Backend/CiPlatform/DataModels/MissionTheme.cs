using System;
using System.Collections.Generic;

namespace CiPlatform.DataModels;

public partial class MissionTheme
{
    public long MissionThemeId { get; set; }

    public string? Title { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Mission> Missions { get; } = new List<Mission>();
}
