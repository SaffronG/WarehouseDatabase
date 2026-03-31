using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("vendor")]
public partial class Vendor
{
    [Key]
    [Column("vendor_id")]
    public int VendorId { get; set; }

    [Column("name")]
    [StringLength(25)]
    public string? Name { get; set; }

    [InverseProperty("Vendor")]
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
