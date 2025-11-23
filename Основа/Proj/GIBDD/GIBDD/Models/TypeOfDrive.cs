using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class TypeOfDrive
{
    public long Todid { get; set; }

    public string? Todname { get; set; }

    public virtual ICollection<Car> CarVimFks { get; set; } = new List<Car>();
}
