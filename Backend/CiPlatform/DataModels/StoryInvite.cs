using System;
using System.Collections.Generic;

namespace CiPlatform.DataModels;

public partial class StoryInvite
{
    public long StoryInviteId { get; set; }

    public long StoryId { get; set; }

    public long FromUserId { get; set; }

    public long ToUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
