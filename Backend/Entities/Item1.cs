using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("item", Schema = "warehouse")]
public partial class Item1
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("item_name")]
    [StringLength(50)]
    public string? ItemName { get; set; }

    [Column("cost")]
    [Precision(10, 2)]
    public decimal? Cost { get; set; }

    [Column("vendor_id")]
    public int? VendorId { get; set; }

    [Column("item_size")]
    [Precision(10, 2)]
    public decimal? ItemSize { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();

    [InverseProperty("Item")]
    public virtual ICollection<ItemShipment> ItemShipments { get; set; } = new List<ItemShipment>();

    [InverseProperty("Item")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("VendorId")]
    [InverseProperty("Item1s")]
    public virtual Vendor1? Vendor { get; set; }
}
