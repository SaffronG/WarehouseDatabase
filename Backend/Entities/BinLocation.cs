using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class BinLocation
{
    public int Id { get; set; }

    public int? ShelfId { get; set; }

    public int? BinId { get; set; }

    public virtual Bin? Bin { get; set; }

    public virtual Shelf? Shelf { get; set; }
}
