using System;
using System.Collections;
using System.Collections.Generic;

namespace AdminHalloDoc.Entities.Models;

public partial class Blockrequest
{
    public int Blockrequestid { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public BitArray? Isactive { get; set; }

    public string? Reason { get; set; }

    public string Requestid { get; set; } = null!;

    public string? Ip { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Modifieddate { get; set; }
}
