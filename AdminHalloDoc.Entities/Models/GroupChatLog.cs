using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdminHalloDoc.Entities.Models;

[Table("GroupChatLog")]
public partial class GroupChatLog
{
    [Key]
    public int GroupChatLogId { get; set; }

    public int AdminId { get; set; }

    [Column(TypeName = "character varying")]
    public string? AdminName { get; set; }

    public int PatientId { get; set; }

    [Column(TypeName = "character varying")]
    public string? PatientName { get; set; }

    public int PhysicianId { get; set; }

    [Column(TypeName = "character varying")]
    public string? PhysicianName { get; set; }

    public int RequestId { get; set; }

    [Column(TypeName = "character varying")]
    public string FilePath { get; set; } = null!;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }
}
