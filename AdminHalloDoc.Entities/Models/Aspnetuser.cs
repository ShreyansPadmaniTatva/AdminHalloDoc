﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminHalloDoc.Entities.Models;

[Table("aspnetusers")]
public partial class Aspnetuser
{
    [Key]
    [Column("id")]
    [StringLength(128)]
    public string Id { get; set; } = null!;

    [Column("username")]
    [StringLength(256)]
    public string Username { get; set; } = null!;

    [Column("passwordhash")]
    [StringLength(255)]
    public string? Passwordhash { get; set; }

    [Column("email")]
    [StringLength(256)]
    public string? Email { get; set; }

    [Column("phonenumber")]
    [StringLength(20)]
    public string? Phonenumber { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [InverseProperty("User")]
    public virtual Aspnetuserrole? Aspnetuserrole { get; set; }

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Business> BusinessCreatedbyNavigations { get; set; } = new List<Business>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Business> BusinessModifiedbyNavigations { get; set; } = new List<Business>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Payratebyprovider> PayratebyproviderCreatedbyNavigations { get; set; } = new List<Payratebyprovider>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Payratebyprovider> PayratebyproviderModifiedbyNavigations { get; set; } = new List<Payratebyprovider>();

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<Physician> PhysicianAspnetusers { get; set; } = new List<Physician>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Physician> PhysicianCreatedbyNavigations { get; set; } = new List<Physician>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Physician> PhysicianModifiedbyNavigations { get; set; } = new List<Physician>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Shiftdetail> Shiftdetails { get; set; } = new List<Shiftdetail>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Timesheet> TimesheetCreatedbyNavigations { get; set; } = new List<Timesheet>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Timesheet> TimesheetModifiedbyNavigations { get; set; } = new List<Timesheet>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Timesheetdetailreimbursement> TimesheetdetailreimbursementCreatedbyNavigations { get; set; } = new List<Timesheetdetailreimbursement>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Timesheetdetailreimbursement> TimesheetdetailreimbursementModifiedbyNavigations { get; set; } = new List<Timesheetdetailreimbursement>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Timesheetdetail> Timesheetdetails { get; set; } = new List<Timesheetdetail>();

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
