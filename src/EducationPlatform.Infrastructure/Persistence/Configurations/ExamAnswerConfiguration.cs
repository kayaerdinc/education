using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPlatform.Infrastructure.Persistence.Configurations;

public class ExamAnswerConfiguration : IEntityTypeConfiguration<ExamAnswer>
{
    public void Configure(EntityTypeBuilder<ExamAnswer> builder)
    {
        builder.ToTable("ExamAnswers");

        builder.HasKey(a => new { a.AttemptId, a.QuestionId });

        builder.Property(a => a.AnsweredAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(a => a.AttemptId);

        builder.HasOne(a => a.Attempt)
               .WithMany(attempt => attempt.Answers)
               .HasForeignKey(a => a.AttemptId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Question)
               .WithMany()
               .HasForeignKey(a => a.QuestionId);

        builder.HasOne(a => a.Choice)
               .WithMany()
               .HasForeignKey(a => a.ChoiceId);
    }
}
