using System.Collections.Generic;

namespace Backend.Entities;

public partial class PurchaseOrder
{
    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
}
