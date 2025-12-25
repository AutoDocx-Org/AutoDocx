using AutoDocx.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoDocx.Infrastructure.Data;

public class AutoDocxDbContext : DbContext
{
    public AutoDocxDbContext(DbContextOptions<AutoDocxDbContext> options) : base(options)
    {
    }

    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateField> TemplateFields { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Template configuration
        modelBuilder.Entity<Template>(entity =>
        {
            entity.ToTable("Templates");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.WordFilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasMany(e => e.Fields)
                .WithOne(e => e.Template)
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // TemplateField configuration
        modelBuilder.Entity<TemplateField>(entity =>
        {
            entity.ToTable("TemplateFields");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FieldKey).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Label).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Placeholder).IsRequired().HasMaxLength(100);
            entity.Property(e => e.OptionsJson).HasColumnType("jsonb");

            entity.HasIndex(e => e.TemplateId);
            entity.HasIndex(e => new { e.TemplateId, e.Order });
        });
    }
}
