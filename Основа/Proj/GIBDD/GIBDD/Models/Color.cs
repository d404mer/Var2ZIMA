using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class Color
{
    public string ColorNum { get; set; } = null!;

    public string? ColorCode { get; set; }

    public string? ColorName1 { get; set; }

    public string? ColorName { get; set; }

    public int? IsMettalic { get; set; }

    public virtual ICollection<Car> CarVimFks { get; set; } = new List<Car>();
}
