// ============================================
// 鍔熻兘鎻忚堪锛氳璇佷笌澶氱鎴锋ā鍧?- EF Core 鏁版嵁搴撲笂涓嬫枃
// 鐢熸垚锛歈oder by 搴勫洯
// 鐢熸垚鏃ユ湡锛?026-07-21
// ============================================

using GrowthAI.Domain.Entities;
using GrowthAI.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GrowthAI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("Tenant");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Industry).HasMaxLength(50);
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.PlanType).HasMaxLength(20).HasDefaultValue("Basic");
            entity.Property(e => e.Status).HasDefaultValue(1);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("AppUser");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(20).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Role).HasMaxLength(20).HasConversion<string>().HasDefaultValue(UserRole.Operator);
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.HasOne(e => e.Tenant).WithMany().HasForeignKey(e => e.TenantId);
            entity.HasIndex(e => new { e.TenantId, e.Phone }).IsUnique();
        });
    }
}