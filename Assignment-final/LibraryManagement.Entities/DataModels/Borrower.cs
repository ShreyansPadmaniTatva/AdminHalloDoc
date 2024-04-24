using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Entities.DataModels;

[Table("borrower")]
public partial class Borrower
{
    [Key]
    [Column("borrowerid")]
    public int Borrowerid { get; set; }

    [Column("borrowername")]
    [StringLength(256)]
    public string Borrowername { get; set; } = null!;

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [InverseProperty("Borrower")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
