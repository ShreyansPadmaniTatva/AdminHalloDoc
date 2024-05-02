﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminHalloDoc.Entities.Models;

[Table("requesttype")]
public partial class Requesttype
{
    [Key]
    [Column("requesttypeid")]
    public int Requesttypeid { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;
}
