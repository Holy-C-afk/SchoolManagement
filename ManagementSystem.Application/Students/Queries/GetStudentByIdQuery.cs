using ManagementSystem.Domain.ValueObjects;

public record GetStudentByIdQuery(StudentId Id)
{
    public Guid StudentId { get; internal set; }
}