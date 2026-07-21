// ============================================
// 功能描述：EF Core 数据库上下文（Sprint 3-9）
// 生成：Qoder by 庄园
// 生成日期：2026-07-21
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
    public DbSet<AiPromptTemplate> AiPromptTemplates => Set<AiPromptTemplate>();
    public DbSet<AiTask> AiTasks => Set<AiTask>();
    public DbSet<AiContent> AiContents => Set<AiContent>();
    public DbSet<LeadCustomer> LeadCustomers => Set<LeadCustomer>();
    public DbSet<FollowUpRecord> FollowUpRecords => Set<FollowUpRecord>();
    public DbSet<LeadSource> LeadSources => Set<LeadSource>();
    public DbSet<LandingPage> LandingPages => Set<LandingPage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // === Tenant ===
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

        // === AppUser ===
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

        // === AiPromptTemplate ===
        modelBuilder.Entity<AiPromptTemplate>(entity =>
        {
            entity.ToTable("AiPromptTemplate");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Industry).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Platform).HasMaxLength(30).IsRequired();
            entity.Property(e => e.ContentType).HasMaxLength(30).IsRequired();
            entity.Property(e => e.PromptText).IsRequired();
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.HasIndex(e => new { e.Industry, e.Platform, e.ContentType });
        });

        // === AiTask ===
        modelBuilder.Entity<AiTask>(entity =>
        {
            entity.ToTable("AiTask");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.TaskType).HasConversion<string>().IsRequired();
            entity.Property(e => e.Status).HasConversion<string>().HasDefaultValue(AiTaskStatus.Queued);
            entity.HasOne(e => e.Tenant).WithMany().HasForeignKey(e => e.TenantId);
            entity.HasOne(e => e.PromptTemplate).WithMany().HasForeignKey(e => e.PromptTemplateId);
            entity.HasIndex(e => new { e.TenantId, e.Status });
        });

        // === AiContent ===
        modelBuilder.Entity<AiContent>(entity =>
        {
            entity.ToTable("AiContent");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.TargetPlatform).HasMaxLength(30);
            entity.Property(e => e.ContentType).HasMaxLength(30);
            entity.Property(e => e.Industry).HasMaxLength(50);
            entity.Property(e => e.Product).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(30);
            entity.Property(e => e.Cta).HasMaxLength(200);
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.HasOne(e => e.Tenant).WithMany().HasForeignKey(e => e.TenantId);
            entity.HasOne(e => e.AiTask).WithMany().HasForeignKey(e => e.AiTaskId);
            entity.HasIndex(e => new { e.TenantId, e.TargetPlatform });
        });

        // === LeadCustomer (Sprint 6) ===
        modelBuilder.Entity<LeadCustomer>(entity =>
        {
            entity.ToTable("LeadCustomer");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.WeChat).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(30);
            entity.Property(e => e.SourcePlatform).HasMaxLength(30);
            entity.Property(e => e.SourceAccount).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("New");
            entity.Property(e => e.SourceType).HasMaxLength(30).HasDefaultValue("ManualInput");
            entity.Property(e => e.InvalidReason).HasMaxLength(200);
            entity.HasOne(e => e.Tenant).WithMany().HasForeignKey(e => e.TenantId);
            entity.HasOne(e => e.AssignedUser).WithMany().HasForeignKey(e => e.AssignedUserId);
            entity.HasIndex(e => new { e.TenantId, e.Status });
            entity.HasIndex(e => new { e.TenantId, e.Phone }).IsUnique();
            entity.HasIndex(e => new { e.TenantId, e.WeChat }).IsUnique();
        });

        // === FollowUpRecord (Sprint 6) ===
        modelBuilder.Entity<FollowUpRecord>(entity =>
        {
            entity.ToTable("FollowUpRecord");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.LeadCustomerId).IsRequired();
            entity.Property(e => e.FollowerId).IsRequired();
            entity.Property(e => e.FollowType).HasMaxLength(20).HasDefaultValue("Phone");
            entity.Property(e => e.Content).IsRequired();
            entity.HasOne(e => e.LeadCustomer).WithMany().HasForeignKey(e => e.LeadCustomerId);
            entity.HasOne(e => e.Follower).WithMany().HasForeignKey(e => e.FollowerId);
            entity.HasIndex(e => new { e.TenantId, e.LeadCustomerId });
        });

        // === LeadSource (Sprint 7) ===
        modelBuilder.Entity<LeadSource>(entity =>
        {
            entity.ToTable("LeadSource");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.SourceType).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Platform).HasMaxLength(30);
            entity.Property(e => e.AccountName).HasMaxLength(100);
            entity.Property(e => e.TrackingCode).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.HasOne(e => e.Tenant).WithMany().HasForeignKey(e => e.TenantId);
            entity.HasIndex(e => e.TrackingCode);
        });

        // === LandingPage (Sprint 9) ===
        modelBuilder.Entity<LandingPage>(entity =>
        {
            entity.ToTable("LandingPage");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TenantId).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.PageCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CoverImage).HasMaxLength(500);
            entity.Property(e => e.ThankYouMessage).HasMaxLength(500);
            entity.Property(e => e.RedirectUrl).HasMaxLength(500);
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.HasOne(e => e.Tenant).WithMany().HasForeignKey(e => e.TenantId);
            entity.HasIndex(e => e.PageCode).IsUnique();
        });
    }
}
