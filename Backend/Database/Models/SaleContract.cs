using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Models;

public partial class SaleContract
{
    [Key]
    public long Id { get; set; }

    public long PropertyId { get; set; }

    public long BuyerId { get; set; }

    public long SellerId { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [ForeignKey("PropertyId")]
    [InverseProperty("SaleContracts")]
    public virtual Property Property { get; set; } = null!;
}
