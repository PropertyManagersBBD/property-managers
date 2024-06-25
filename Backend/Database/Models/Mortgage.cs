using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Models;

public partial class Mortgage
{
    [Key]
    public long Id { get; set; }

    public long PropertyId { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("PropertyId")]
    [InverseProperty("Mortgages")]
    public virtual Property Property { get; set; } = null!;
}
