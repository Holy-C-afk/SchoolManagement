using Microsoft.EntityFrameworkCore;           // Pour IEntityTypeConfiguration
using Microsoft.EntityFrameworkCore.Metadata.Builders; // Pour EntityTypeBuilder
using ManagementSystem.Domain.Entities;         // Pour trouver la classe Student
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.FullName).IsRequired().HasMaxLength(200);
        builder.HasMany(s => s.Exams).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}
