using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminHalloDoc.Entities.Models;

[Table("aspnetuserroles")]
public partial class Aspnetuserrole
{
    [Key]
    [Column("userid")]
    [StringLength(128)]
    public string Userid { get; set; } = null!;

    [Column("roleid")]
    [StringLength(128)]
    public string Roleid { get; set; } = null!;

    [ForeignKey("Userid")]
    [InverseProperty("Aspnetuserrole")]
    public virtual Aspnetuser User { get; set; } = null!;
}
