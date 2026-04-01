using System;
using System.Collections.Generic;

namespace Backend.Entities;

public partial class Aisle
{
    public int Id { get; set; }

    public int? Height { get; set; }

    public int? Width { get; set; }

    public int? Length { get; set; }

    public int? MaxBays { get; set; }

    public int? MaxShelves { get; set; }

    public virtual ICollection<Bay> Bays { get; set; } = new List<Bay>();
}
