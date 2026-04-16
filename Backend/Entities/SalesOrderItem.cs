using System;

namespace Backend.Entities;

public partial class SalesOrderItem
{
    public int Id { get; set; }
    public int SalesOrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }

    public virtual Item? Item { get; set; }
    public virtual Order? Order { get; set; }
}
