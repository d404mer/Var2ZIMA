using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class Manufacturer
{
    public long ManufacturerId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public virtual ICollection<Car> CarVimFks { get; set; } = new List<Car>();
}
