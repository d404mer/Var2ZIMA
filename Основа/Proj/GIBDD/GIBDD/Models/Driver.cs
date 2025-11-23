using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class Driver
{
    public long Guid { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string PassportSeries { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public string Postcode { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string ActualAdress { get; set; } = null!;

    public string? Company { get; set; }

    public string? JobName { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Photo { get; set; } = null!;

    public string? Description { get; set; }

    public string LicenceDate { get; set; } = null!;

    public string ExpireDate { get; set; } = null!;

    public string Categories { get; set; } = null!;

    public string LicenceSeries { get; set; } = null!;

    public string LicenceNumber { get; set; } = null!;

    public string LicenceStatus { get; set; } = null!;

    public virtual ICollection<AccidentParticipant> AccidentParticipants { get; set; } = new List<AccidentParticipant>();

    public virtual ICollection<Car> CarVimFks { get; set; } = new List<Car>();
}
