using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("order_item", Schema = "warehouse")]
public partial class OrderItem
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

    [Column("unit_price")]
    [Precision(10, 2)]
    public decimal? UnitPrice { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("OrderItems")]
    public virtual Item1? Item { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual Order? Order { get; set; }
}
