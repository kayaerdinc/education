using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPlatform.Infrastructure.Persistence.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("Videos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(v => v.Description)
               .HasMaxLength(1000);

        builder.Property(v => v.Url)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(v => v.OrderIndex)
               .IsRequired();

        builder.HasIndex(v => v.OrderIndex)
               .IsUnique();

        builder.Property(v => v.DurationSeconds);

        builder.Property(v => v.ThumbnailUrl)
               .HasMaxLength(500);

        builder.Property(v => v.CreatedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}
