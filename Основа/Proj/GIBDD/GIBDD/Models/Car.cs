using System;
using System.Collections.Generic;

namespace GIBDD.Models;

/// <summary>
/// Модель автомобиля
/// </summary>
public partial class Car
{
    /// <summary>
    /// VIN номер автомобиля (уникальный идентификатор)
    /// </summary>
    public string CarVim { get; set; } = null!;

    /// <summary>
    /// Модель автомобиля
    /// </summary>
    public string? CarModel { get; set; }

    /// <summary>
    /// Год выпуска автомобиля
    /// </summary>
    public string? CarYear { get; set; }

    /// <summary>
    /// Вес автомобиля
    /// </summary>
    public string? CarWeight { get; set; }

    /// <summary>
    /// Коллекция типов двигателей, связанных с автомобилем
    /// </summary>
    public virtual ICollection<EngineType> CarVimFks { get; set; } = new List<EngineType>();

    /// <summary>
    /// Коллекция цветов, связанных с автомобилем
    /// </summary>
    public virtual ICollection<Color> ColorNumFks { get; set; } = new List<Color>();

    /// <summary>
    /// Коллекция водителей, связанных с автомобилем
    /// </summary>
    public virtual ICollection<Driver> GuidFks { get; set; } = new List<Driver>();

    /// <summary>
    /// Коллекция производителей, связанных с автомобилем
    /// </summary>
    public virtual ICollection<Manufacturer> ManufacturerIdFks { get; set; } = new List<Manufacturer>();

    /// <summary>
    /// Коллекция регионов, связанных с автомобилем
    /// </summary>
    public virtual ICollection<Region> RegionIdFks { get; set; } = new List<Region>();

    /// <summary>
    /// Коллекция типов привода, связанных с автомобилем
    /// </summary>
    public virtual ICollection<TypeOfDrive> TodidFks { get; set; } = new List<TypeOfDrive>();
}
