using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPlatform.Infrastructure.Persistence.Configurations;

public class StudentVideoProgressConfiguration : IEntityTypeConfiguration<StudentVideoProgress>
{
    public void Configure(EntityTypeBuilder<StudentVideoProgress> builder)
    {
        builder.ToTable("StudentVideoProgress");

        builder.HasKey(p => new { p.UserId, p.VideoId });

        builder.Property(p => p.CompletedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(p => p.User)
               .WithMany(u => u.VideoProgresses)
               .HasForeignKey(p => p.UserId);

        builder.HasOne(p => p.Video)
               .WithMany(v => v.ProgressRecords)
               .HasForeignKey(p => p.VideoId);
    }
}
