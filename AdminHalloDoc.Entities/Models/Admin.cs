using System;
using System.Collections;
using System.Collections.Generic;

namespace AdminHalloDoc.Entities.Models;

public partial class Admin
{
    public int Adminid { get; set; }

    public string Aspnetuserid { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public int? Regionid { get; set; }

    public string? Zip { get; set; }

    public string? Altphone { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public short? Status { get; set; }

    public BitArray? Isdeleted { get; set; }

    public int? Roleid { get; set; }

    public virtual ICollection<Adminregion> Adminregions { get; set; } = new List<Adminregion>();

    public virtual ICollection<Requeststatuslog> Requeststatuslogs { get; set; } = new List<Requeststatuslog>();

    public virtual ICollection<Requestwisefile> Requestwisefiles { get; set; } = new List<Requestwisefile>();
}
