using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

public sealed class Enrollment 
{
    // Ajoute ces deux lignes
    public StudentId StudentId { get; private set; }
    public CourseId CourseId { get; private set; }

    public Student Student { get; private set; }
    public Course Course { get; private set; }
    public DateTime EnrolledAt { get; private set; }

    // Un constructeur privé pour EF Core
    private Enrollment() { }

    // Ton constructeur pour créer une inscription
    public Enrollment(StudentId studentId, CourseId courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
        EnrolledAt = DateTime.UtcNow;
    }
}