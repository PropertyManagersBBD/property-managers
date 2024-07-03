using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Models;

public partial class RentalContract
{
    [Key]
    public long Id { get; set; }

    public long PropertyId { get; set; }

    public long LandlordId { get; set; }

    public long TenantId { get; set; }

    public long Rent { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("PropertyId")]
    [InverseProperty("RentalContracts")]
    public virtual Property Property { get; set; } = null!;
}
