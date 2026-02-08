using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(
                id => id.Value,
                value => new ExamId(value)
            );

        builder.Property(e => e.Subject)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(e => e.Score, score =>
        {
            score.Property(s => s.Value)
                 .HasColumnName("Score")
                 .IsRequired();
        });
    }
}
