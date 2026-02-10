namespace ManagementSystem.API.Models.Responses;

public sealed class StudentResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
       public DateTime BirthDate { get; init; }
}