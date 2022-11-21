using System;
using System.Collections.Generic;

namespace CiPlatform.DataModels;

public partial class City
{
    public long CityId { get; set; }

    public long CountryId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? StateId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual State? State { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
