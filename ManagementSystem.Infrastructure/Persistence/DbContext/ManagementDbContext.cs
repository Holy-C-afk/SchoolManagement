using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

public class ManagementDbContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<User> Users { get; set; }

    public ManagementDbContext(DbContextOptions<ManagementDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configuration pour l'entité Student
    modelBuilder.Entity<Student>(builder =>
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,           // Dit à EF : stocke le Guid dans SQL
                value => new StudentId(value)); // Dit à EF : recrée le StudentId en C#
    });

    // Configuration pour l'entité Course
    modelBuilder.Entity<Course>(builder =>
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => new CourseId(value));
    });

    // Configuration pour l'entité Department
    modelBuilder.Entity<Department>(builder =>
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => new DepartmentId(value));
    });
    modelBuilder.Entity<Exam>(builder =>
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasConversion(
                id => id.Value,
                value => new ExamId(value));
    });
    modelBuilder.Entity<Teacher>(builder =>
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => new TeacherId(value));
    });
    modelBuilder.Entity<Enrollment>(builder =>
{
    // On définit une clé composée (StudentId + CourseId)
    builder.HasKey(e => new { e.StudentId, e.CourseId });

    // On n'oublie pas de configurer les conversions pour ces deux IDs typés
    builder.Property(e => e.StudentId)
        .HasConversion(
            id => id.Value,
            value => new StudentId(value));

    builder.Property(e => e.CourseId)
        .HasConversion(
            id => id.Value,
            value => new CourseId(value));
});
    


    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementDbContext).Assembly);
}
}
