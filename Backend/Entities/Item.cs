using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("item")]
public partial class Item
{
    [Key]
    [Column("item_id")]
    public int ItemId { get; set; }

    [Column("item_name")]
    [StringLength(50)]
    public string? ItemName { get; set; }

    [Column("cost")]
    [Precision(10, 2)]
    public decimal? Cost { get; set; }

    [Column("vendor_id")]
    public int? VendorId { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<ItemsInShipmentOrder> ItemsInShipmentOrders { get; set; } = new List<ItemsInShipmentOrder>();

    [InverseProperty("Item")]
    public virtual ICollection<ItemsPurchaseOrder> ItemsPurchaseOrders { get; set; } = new List<ItemsPurchaseOrder>();

    [ForeignKey("VendorId")]
    [InverseProperty("Items")]
    public virtual Vendor? Vendor { get; set; }
}
