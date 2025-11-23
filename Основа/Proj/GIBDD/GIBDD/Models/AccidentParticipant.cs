using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class AccidentParticipant
{
    public long PartId { get; set; }

    public long AccidentIdFk { get; set; }

    public long? GuidFk { get; set; }

    public string? Role { get; set; }

    public virtual Accident AccidentIdFkNavigation { get; set; } = null!;

    public virtual Driver? GuidFkNavigation { get; set; }
}
