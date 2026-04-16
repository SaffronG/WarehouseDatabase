using System;

namespace Backend.Entities;

public partial class Order
{
    public string Status { get; set; } = OrderStatus.Created.ToString().ToUpperInvariant();
    public DateTime? DatePicked { get; set; }
    public DateTime? DatePacked { get; set; }
    public DateTime? DateShipped { get; set; }
    public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
}
