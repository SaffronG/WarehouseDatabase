using System;

namespace Backend.Entities;

public partial class PurchaseOrderItem
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }

    public virtual Item? Item { get; set; }
    public virtual PurchaseOrder? PurchaseOrder { get; set; }
}
