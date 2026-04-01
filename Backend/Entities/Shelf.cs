using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class Shelf
{
    public int Id { get; set; }

    public int? BayId { get; set; }

    public int? ShelfNumber { get; set; }

    public virtual Bay? Bay { get; set; }

    public virtual ICollection<BinLocation> BinLocations { get; set; } = new List<BinLocation>();
}
