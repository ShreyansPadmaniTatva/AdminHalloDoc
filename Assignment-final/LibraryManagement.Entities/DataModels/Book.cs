using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Entities.DataModels;

[Table("book")]
public partial class Book
{
    [Key]
    [Column("bookid")]
    public int Bookid { get; set; }

    [Column("bookname")]
    [StringLength(500)]
    public string Bookname { get; set; } = null!;

    [Column("author")]
    [StringLength(500)]
    public string? Author { get; set; }

    [Column("dateofissue", TypeName = "timestamp without time zone")]
    public DateTime? Dateofissue { get; set; }

    [Column("genere")]
    public short? Genere { get; set; }

    [Column("borrowerid")]
    public int Borrowerid { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [ForeignKey("Borrowerid")]
    [InverseProperty("Books")]
    public virtual Borrower Borrower { get; set; } = null!;
}
