using System;
using System.Collections.Generic;

namespace AdminHalloDoc.Entities.Models;

public partial class Rolemenu
{
    public int Rolemenuid { get; set; }

    public int Roleid { get; set; }

    public int Menuid { get; set; }

    public virtual Menu Menu { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
