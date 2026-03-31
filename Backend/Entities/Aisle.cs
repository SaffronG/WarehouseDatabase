using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("aisle", Schema = "warehouse")]
public partial class Aisle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("height")]
    public int? Height { get; set; }

    [Column("width")]
    public int? Width { get; set; }

    [Column("length")]
    public int? Length { get; set; }

    [Column("max_bays")]
    public int? MaxBays { get; set; }

    [Column("max_shelves")]
    public int? MaxShelves { get; set; }

    [InverseProperty("Aisle")]
    public virtual ICollection<Bay> Bays { get; set; } = new List<Bay>();
}
