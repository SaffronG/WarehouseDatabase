using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class ItemShipment
{
    public int Id { get; set; }

    public int? ItemId { get; set; }

    public int ShipmentId { get; set; }

    public int? ActionId { get; set; }

    public decimal? Discount { get; set; }

    public int? Quantity { get; set; }

    public virtual ShipmentAction? Action { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Shipment Shipment { get; set; } = null!;
}
