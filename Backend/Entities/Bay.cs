using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class Bay
{
    public int Id { get; set; }

    public int? AisleId { get; set; }

    public virtual Aisle? Aisle { get; set; }

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}
