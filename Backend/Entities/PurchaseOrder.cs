using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("purchase_order")]
public partial class PurchaseOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("handling_cost")]
    [Precision(10, 2)]
    public decimal? HandlingCost { get; set; }

    [Column("date_ordered", TypeName = "timestamp without time zone")]
    public DateTime? DateOrdered { get; set; }

    [Column("date_recieved", TypeName = "timestamp without time zone")]
    public DateTime? DateRecieved { get; set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<ItemsInShipmentOrder> ItemsInShipmentOrders { get; set; } = new List<ItemsInShipmentOrder>();

    [InverseProperty("Order")]
    public virtual ICollection<ItemsPurchaseOrder> ItemsPurchaseOrders { get; set; } = new List<ItemsPurchaseOrder>();
}
