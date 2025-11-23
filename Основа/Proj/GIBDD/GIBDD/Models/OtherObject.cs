using System;
using System.Collections.Generic;

namespace GIBDD.Models;

public partial class OtherObject
{
    public long ObjId { get; set; }

    public long? AccidentIdFk { get; set; }

    public string? ObjectType { get; set; }

    public string? Comments { get; set; }

    public virtual Accident? AccidentIdFkNavigation { get; set; }
}
