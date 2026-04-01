using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class Shipment
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public DateTime? DateReceived { get; set; }

    public decimal? PriceAdjust { get; set; }

    public decimal? HandlingCost { get; set; }

    public virtual ICollection<ItemShipment> ItemShipments { get; set; } = new List<ItemShipment>();

    public virtual PurchaseOrder? Order { get; set; }
}
