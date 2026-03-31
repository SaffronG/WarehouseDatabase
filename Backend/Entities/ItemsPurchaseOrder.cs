using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("items_purchase_order")]
public partial class ItemsPurchaseOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("item_id")]
    public int? ItemId { get; set; }

    [Column("order_id")]
    public int? OrderId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemsPurchaseOrders")]
    public virtual Item? Item { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("ItemsPurchaseOrders")]
    public virtual PurchaseOrder? Order { get; set; }
}
