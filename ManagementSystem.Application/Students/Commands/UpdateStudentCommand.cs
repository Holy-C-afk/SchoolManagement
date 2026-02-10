namespace ManagementSystem.Application.Students;

public sealed class UpdateStudentCommand
{
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public DateOnly BirthDate { get; }

    public UpdateStudentCommand(Guid id, string firstName, string lastName , DateOnly birthDate)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }
}
