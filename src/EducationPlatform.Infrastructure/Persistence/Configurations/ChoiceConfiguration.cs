using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPlatform.Infrastructure.Persistence.Configurations;

public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
{
    public void Configure(EntityTypeBuilder<Choice> builder)
    {
        builder.ToTable("Choices");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.QuestionId)
               .IsRequired();

        builder.Property(c => c.ChoiceLabel)
               .IsRequired()
               .HasMaxLength(1)
               .IsFixedLength();

        builder.Property(c => c.Body)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(c => c.IsCorrect)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(c => new { c.QuestionId, c.ChoiceLabel })
               .IsUnique();

        builder.HasOne(c => c.Question)
               .WithMany(q => q.Choices)
               .HasForeignKey(c => c.QuestionId);
    }
}
