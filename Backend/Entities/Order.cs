using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("order", Schema = "warehouse")]
public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("date_ordered", TypeName = "timestamp without time zone")]
    public DateTime? DateOrdered { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Order")]
    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
