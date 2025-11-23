using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class Car
{
    public string CarVim { get; set; } = null!;

    public string? CarModel { get; set; }

    public string? CarYear { get; set; }

    public string? CarWeight { get; set; }

    public virtual ICollection<EngineType> CarVimFks { get; set; } = new List<EngineType>();

    public virtual ICollection<Color> ColorNumFks { get; set; } = new List<Color>();

    public virtual ICollection<Driver> GuidFks { get; set; } = new List<Driver>();

    public virtual ICollection<Manufacturer> ManufacturerIdFks { get; set; } = new List<Manufacturer>();

    public virtual ICollection<Region> RegionIdFks { get; set; } = new List<Region>();

    public virtual ICollection<TypeOfDrive> TodidFks { get; set; } = new List<TypeOfDrive>();
}
