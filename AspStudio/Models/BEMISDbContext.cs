using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AspStudio.Models;

public partial class BEMISDbContext : DbContext
{
    public BEMISDbContext()
    {
    }

    public BEMISDbContext(DbContextOptions<BEMISDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }

    public virtual DbSet<MainMenu> MainMenu { get; set; }

    public virtual DbSet<SubMenu> SubMenu { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-1I9KQQP;Database=BEMIS;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRoles>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");
        });

        modelBuilder.Entity<MainMenu>(entity =>
        {
            entity.Property(e => e.MenuIcon).HasDefaultValue("");
        });

        modelBuilder.Entity<SubMenu>(entity =>
        {
            entity.HasOne(d => d.MainMenu).WithMany(p => p.SubMenu).OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
