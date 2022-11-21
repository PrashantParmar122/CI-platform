using System;
using System.Collections.Generic;

namespace CiPlatform.DataModels;

public partial class Country
{
    public long CountryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Iso { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual ICollection<Mission> Missions { get; } = new List<Mission>();

    public virtual ICollection<State> States { get; } = new List<State>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
