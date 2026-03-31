using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("vendor", Schema = "warehouse")]
public partial class Vendor1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(25)]
    public string? Name { get; set; }

    [InverseProperty("Vendor")]
    public virtual ICollection<Item1> Item1s { get; set; } = new List<Item1>();
}
