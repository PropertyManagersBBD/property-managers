using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Models;

public partial class Property
{
    [Key]
    public long Id { get; set; }

    public long? OwnerId { get; set; }

    public int Capacity { get; set; }

    public bool ListedForRent { get; set; }

    public bool ListedForSale { get; set; }

    [InverseProperty("Property")]
    public virtual ICollection<Mortgage> Mortgages { get; set; } = new List<Mortgage>();

    [InverseProperty("Property")]
    public virtual ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();

    [InverseProperty("Property")]
    public virtual ICollection<SaleContract> SaleContracts { get; set; } = new List<SaleContract>();
}
