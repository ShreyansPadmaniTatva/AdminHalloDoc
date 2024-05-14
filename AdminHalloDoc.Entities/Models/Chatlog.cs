using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdminHalloDoc.Entities.Models;

[Table("chatlog")]
public partial class Chatlog
{
    [Key]
    [Column("chatlogid")]
    public int Chatlogid { get; set; }

    [Column("senderid")]
    public int Senderid { get; set; }

    [Column("sendername", TypeName = "character varying")]
    public string Sendername { get; set; } = null!;

    [Column("sendertype", TypeName = "character varying")]
    public string Sendertype { get; set; } = null!;

    [Column("recieverid")]
    public int Recieverid { get; set; }

    [Column("recievername", TypeName = "character varying")]
    public string Recievername { get; set; } = null!;

    [Column("receivertype", TypeName = "character varying")]
    public string Receivertype { get; set; } = null!;

    [Column("requestid")]
    public int Requestid { get; set; }

    [Column("filepath", TypeName = "character varying")]
    public string Filepath { get; set; } = null!;

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }
}
