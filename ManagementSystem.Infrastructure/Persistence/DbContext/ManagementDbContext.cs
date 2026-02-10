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

        // =========================
        // Student (SEULE PARTIE MODIFIÉE)
        // =========================
        modelBuilder.Entity<Student>(builder =>
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasConversion(
                    id => id.Value,
                    value => new StudentId(value))
                .ValueGeneratedNever();

            // Relation Student -> Exams (sans navigation inverse)
            builder.HasMany(s => s.Exams)
                   .WithOne()
                   .HasForeignKey("StudentId")
                   .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // Course (INCHANGÉ)
        // =========================
        modelBuilder.Entity<Course>(builder =>
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasConversion(
                    id => id.Value,
                    value => new CourseId(value));
        });

        // =========================
        // Department (INCHANGÉ)
        // =========================
        modelBuilder.Entity<Department>(builder =>
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id)
                .HasConversion(
                    id => id.Value,
                    value => new DepartmentId(value));
        });

        // =========================
        // Exam (INCHANGÉ)
        // =========================
        modelBuilder.Entity<Exam>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ExamId(value));
        });

        // =========================
        // Teacher (INCHANGÉ)
        // =========================
        modelBuilder.Entity<Teacher>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .HasConversion(
                    id => id.Value,
                    value => new TeacherId(value));
        });

        // =========================
        // Enrollment (INCHANGÉ)
        // =========================
        modelBuilder.Entity<Enrollment>(builder =>
        {
            builder.HasKey(e => new { e.StudentId, e.CourseId });

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
