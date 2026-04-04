using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class PurchaseOrder
{
    public int Id { get; set; }

    public DateTime? DateOrdered { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}