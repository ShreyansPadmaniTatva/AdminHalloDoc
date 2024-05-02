using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminHalloDoc.Entities.Models;

[Table("aspnetroles")]
public partial class Aspnetrole
{
    [Key]
    [Column("id")]
    [StringLength(128)]
    public string Id { get; set; } = null!;

    [Column("name")]
    [StringLength(256)]
    public string Name { get; set; } = null!;
}
