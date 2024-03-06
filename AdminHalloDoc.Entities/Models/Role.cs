using System;
using System.Collections;
using System.Collections.Generic;

namespace AdminHalloDoc.Entities.Models;

public partial class Role
{
    public int Roleid { get; set; }

    public string Name { get; set; } = null!;

    public short Accounttype { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public BitArray Isdeleted { get; set; } = null!;

    public string? Ip { get; set; }

    public virtual ICollection<Rolemenu> Rolemenus { get; set; } = new List<Rolemenu>();
}
