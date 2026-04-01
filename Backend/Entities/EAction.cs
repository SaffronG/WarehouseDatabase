using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class EAction
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ItemShipment> ItemShipments { get; set; } = new List<ItemShipment>();
}
