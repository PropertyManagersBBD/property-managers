using System;
using System.Collections.Generic;
using Backend.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database.Context;

public partial class PropertyManagerContext : DbContext
{
    public PropertyManagerContext()
    {
    }

    public PropertyManagerContext(DbContextOptions<PropertyManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Mortgage> Mortgages { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<RentalContract> RentalContracts { get; set; }

    public virtual DbSet<SaleContract> SaleContracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mortgage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mortgage__3214EC078AFEFE4F");

            entity.HasOne(d => d.Property).WithMany(p => p.Mortgages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mortgages_Properties");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Properti__3214EC07FDD2CDC5");
        });

        modelBuilder.Entity<RentalContract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RentalCo__3214EC073F6D67CB");

            entity.HasOne(d => d.Property).WithMany(p => p.RentalContracts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RentalContracts_Properties");
        });

        modelBuilder.Entity<SaleContract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SaleCont__3214EC076191478C");

            entity.HasOne(d => d.Property).WithMany(p => p.SaleContracts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleContracts_Properties");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
