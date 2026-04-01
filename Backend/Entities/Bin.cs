using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class Bin
{
    public int Id { get; set; }

    public int? ItemId { get; set; }

    public int? BinType { get; set; }

    public int? Quantity { get; set; }

    public virtual ICollection<BinLocation> BinLocations { get; set; } = new List<BinLocation>();

    public virtual BinType? BinTypeNavigation { get; set; }

    public virtual Item? Item { get; set; }
}
