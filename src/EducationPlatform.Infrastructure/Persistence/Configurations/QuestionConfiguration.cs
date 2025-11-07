using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPlatform.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Body)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(q => q.DisplayOrder)
               .IsRequired();

        builder.HasIndex(q => q.DisplayOrder)
               .IsUnique();

        builder.Property(q => q.CreatedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");
    }
}
