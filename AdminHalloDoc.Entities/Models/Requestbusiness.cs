using System;
using System.Collections.Generic;

namespace AdminHalloDoc.Entities.Models;

public partial class Requestbusiness
{
    public int Requestbusinessid { get; set; }

    public int? Requestid { get; set; }

    public int? Businessid { get; set; }

    public string? Ip { get; set; }

    public virtual Business? Business { get; set; }

    public virtual Request? Request { get; set; }
}
