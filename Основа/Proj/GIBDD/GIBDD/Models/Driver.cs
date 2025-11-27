using System;
using System.Collections.Generic;

namespace GIBDD.Models;

/// <summary>
/// Модель водителя
/// </summary>
public partial class Driver
{
    /// <summary>
    /// Уникальный идентификатор водителя
    /// </summary>
    public long Guid { get; set; }

    /// <summary>
    /// Имя водителя
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Фамилия водителя
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// Отчество водителя
    /// </summary>
    public string MiddleName { get; set; } = null!;

    /// <summary>
    /// Серия паспорта
    /// </summary>
    public string PassportSeries { get; set; } = null!;

    /// <summary>
    /// Номер паспорта
    /// </summary>
    public string PassportNumber { get; set; } = null!;

    /// <summary>
    /// Почтовый индекс
    /// </summary>
    public string Postcode { get; set; } = null!;

    /// <summary>
    /// Адрес регистрации
    /// </summary>
    public string Address { get; set; } = null!;

    /// <summary>
    /// Адрес фактического проживания
    /// </summary>
    public string ActualAdress { get; set; } = null!;

    /// <summary>
    /// Место работы
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Должность
    /// </summary>
    public string? JobName { get; set; }

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string Phone { get; set; } = null!;

    /// <summary>
    /// Email адрес
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Путь к фотографии
    /// </summary>
    public string Photo { get; set; } = null!;

    /// <summary>
    /// Замечания и примечания
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Дата выдачи водительского удостоверения
    /// </summary>
    public string LicenceDate { get; set; } = null!;

    /// <summary>
    /// Дата окончания срока действия водительского удостоверения
    /// </summary>
    public string ExpireDate { get; set; } = null!;

    /// <summary>
    /// Категории водительского удостоверения
    /// </summary>
    public string Categories { get; set; } = null!;

    /// <summary>
    /// Серия водительского удостоверения
    /// </summary>
    public string LicenceSeries { get; set; } = null!;

    /// <summary>
    /// Номер водительского удостоверения
    /// </summary>
    public string LicenceNumber { get; set; } = null!;

    /// <summary>
    /// Статус водительского удостоверения
    /// </summary>
    public string LicenceStatus { get; set; } = null!;

    /// <summary>
    /// Коллекция участников ДТП, связанных с водителем
    /// </summary>
    public virtual ICollection<AccidentParticipant> AccidentParticipants { get; set; } = new List<AccidentParticipant>();

    /// <summary>
    /// Коллекция автомобилей, связанных с водителем
    /// </summary>
    public virtual ICollection<Car> CarVimFks { get; set; } = new List<Car>();
}
