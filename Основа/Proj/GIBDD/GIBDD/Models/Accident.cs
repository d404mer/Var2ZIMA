using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class Accident
{
    public long AccidentId { get; set; }

    public string AccidentAddress { get; set; } = null!;

    public string VehiclesList { get; set; } = null!;

    public string ObjectsList { get; set; } = null!;

    public string? Comments { get; set; }

    public DateTime AccidentDate { get; set; }

    public string AccidentClassification { get; set; } = null!;

    public DateTime AccidentTime { get; set; }

    public int VictimsCount { get; set; }

    public string CreatedAt { get; set; } = null!;

    public virtual ICollection<AccidentParticipant> AccidentParticipants { get; set; } = new List<AccidentParticipant>();

    public virtual ICollection<OtherObject> OtherObjects { get; set; } = new List<OtherObject>();
}
