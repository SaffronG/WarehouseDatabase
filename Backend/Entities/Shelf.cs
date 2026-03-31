using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("shelf", Schema = "warehouse")]
public partial class Shelf
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("bay_id")]
    public int? BayId { get; set; }

    [Column("shelf_number")]
    public int? ShelfNumber { get; set; }

    [ForeignKey("BayId")]
    [InverseProperty("Shelves")]
    public virtual Bay? Bay { get; set; }

    [InverseProperty("Shelf")]
    public virtual ICollection<BinLocation> BinLocations { get; set; } = new List<BinLocation>();
}
