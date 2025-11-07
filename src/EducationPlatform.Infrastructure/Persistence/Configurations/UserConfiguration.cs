using EducationPlatform.Domain.Entities;
using EducationPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPlatform.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasColumnName("Id");

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.Property(u => u.PasswordHash)
               .IsRequired()
               .HasColumnType("varbinary(64)");

        builder.Property(u => u.PasswordSalt)
               .IsRequired()
               .HasColumnType("varbinary(32)");

        builder.Property(u => u.Role)
               .IsRequired()
               .HasConversion(
                   role => role.ToString(),
                   value => Enum.Parse<UserRole>(value))
               .HasMaxLength(32);

        builder.Property(u => u.FullName)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(u => u.IsActive)
               .IsRequired()
               .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
               .HasColumnType("datetime2(3)")
               .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(u => u.UpdatedAt)
               .HasColumnType("datetime2(3)");

        builder.HasMany(u => u.VideoProgresses)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId);

        builder.HasMany(u => u.ExamAttempts)
               .WithOne(a => a.User)
               .HasForeignKey(a => a.UserId);
    }
}
