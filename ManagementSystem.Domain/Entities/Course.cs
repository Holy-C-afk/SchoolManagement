using ManagementSystem.Domain.ValueObjects; namespace ManagementSystem.Domain.Entities;
public sealed class Course 
{
    public CourseId Id { get; private set; }
    public string Name { get; private set; }
    public Teacher Teacher { get; private set; }
}
