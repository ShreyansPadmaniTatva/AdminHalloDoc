using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdminHalloDoc.Entities.Models;

[Table("pushnotificationdata")]
public partial class Pushnotificationdatum
{
    [Key]
    [Column("pushnotificationid")]
    public int Pushnotificationid { get; set; }

    [Column("clientname")]
    [StringLength(256)]
    public string Clientname { get; set; } = null!;

    [Column("endpoint")]
    public string Endpoint { get; set; } = null!;

    [Column("p256dh")]
    public string P256dh { get; set; } = null!;

    [Column("auth")]
    public string Auth { get; set; } = null!;

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }
}
