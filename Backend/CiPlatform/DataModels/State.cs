using System;
using System.Collections.Generic;

namespace CiPlatform.DataModels;

public partial class State
{
    public long StateId { get; set; }

    public long CountryId { get; set; }

    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual Country Country { get; set; } = null!;
}
