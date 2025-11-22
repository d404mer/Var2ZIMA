using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class Region
{
    public long RegionId { get; set; }

    public string? RegionName { get; set; }

    public string? RegionCode { get; set; }

    public virtual ICollection<Car> CarVimFks { get; set; } = new List<Car>();
}
