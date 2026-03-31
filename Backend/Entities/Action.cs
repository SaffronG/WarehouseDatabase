using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("action", Schema = "warehouse")]
public partial class Action
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(15)]
    public string? Name { get; set; }

    [InverseProperty("Action")]
    public virtual ICollection<ItemShipment> ItemShipments { get; set; } = new List<ItemShipment>();
}
