using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPlatform.Infrastructure.Persistence.Configurations;

public class ExamAttemptConfiguration : IEntityTypeConfiguration<ExamAttempt>
{
    public void Configure(EntityTypeBuilder<ExamAttempt> builder)
    {
        builder.ToTable("ExamAttempts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserId)
               .IsRequired();

        builder.Property(a => a.StartedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(a => a.CompletedAt)
               .HasColumnType("datetime2(3)");

        builder.Property(a => a.TotalCorrect);
        builder.Property(a => a.TotalIncorrect);

        builder.HasIndex(a => a.UserId);

        builder.HasOne(a => a.User)
               .WithMany(u => u.ExamAttempts)
               .HasForeignKey(a => a.UserId);

        builder.HasOne(a => a.Survey)
               .WithOne(s => s.Attempt)
               .HasForeignKey<Survey>(s => s.AttemptId);
    }
}
