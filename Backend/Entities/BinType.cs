using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class BinType
{
    public int Id { get; set; }

    public int? Height { get; set; }

    public int? Length { get; set; }

    public int? Width { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();
}
