using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Entities;

[Table("store_customer")]
public partial class StoreCustomer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customername")]
    [StringLength(30)]
    public string? Customername { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<StoreCheckout> StoreCheckouts { get; set; } = new List<StoreCheckout>();
}
