using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPlatform.Infrastructure.Persistence.Configurations;

public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder)
    {
        builder.ToTable("Surveys");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.AttemptId)
               .IsRequired();

        builder.HasIndex(s => s.AttemptId)
               .IsUnique();

        builder.Property(s => s.Liked)
               .IsRequired();

        builder.Property(s => s.ContentClarityRating)
               .IsRequired();

        builder.Property(s => s.InstructorSupportRating)
               .IsRequired();

        builder.Property(s => s.AdditionalFeedback)
               .HasMaxLength(1000);

        builder.Property(s => s.SubmittedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasOne(s => s.Attempt)
               .WithOne(a => a.Survey)
               .HasForeignKey<Survey>(s => s.AttemptId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
