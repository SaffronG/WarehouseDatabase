using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? ItemId { get; set; }

    public int? OrderId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual Item? Item { get; set; }

    public virtual PurchaseOrder? Order { get; set; }
}
