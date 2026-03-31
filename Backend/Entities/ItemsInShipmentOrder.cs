using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("items_in_shipment_order")]
public partial class ItemsInShipmentOrder
{
    [Key]
    [Column("items_inship_order_id")]
    public int ItemsInshipOrderId { get; set; }

    [Column("item_id")]
    public int? ItemId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("purchase_order_id")]
    public int? PurchaseOrderId { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemsInShipmentOrders")]
    public virtual Item? Item { get; set; }

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("ItemsInShipmentOrders")]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }
}
