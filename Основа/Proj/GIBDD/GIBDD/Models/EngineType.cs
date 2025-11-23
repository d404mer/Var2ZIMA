using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class EngineType
{
    public string EngineId { get; set; } = null!;

    public string? EngineName { get; set; }

    public virtual ICollection<Car> EngineIdFks { get; set; } = new List<Car>();
}
