using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("bin_location", Schema = "warehouse")]
public partial class BinLocation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shelf_id")]
    public int? ShelfId { get; set; }

    [Column("bin_id")]
    public int? BinId { get; set; }

    [ForeignKey("BinId")]
    [InverseProperty("BinLocations")]
    public virtual Bin? Bin { get; set; }

    [ForeignKey("ShelfId")]
    [InverseProperty("BinLocations")]
    public virtual Shelf? Shelf { get; set; }
}
