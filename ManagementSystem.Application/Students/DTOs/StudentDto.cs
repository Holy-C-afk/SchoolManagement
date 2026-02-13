namespace ManagementSystem.Application.Students.DTOs
{
public class StudentDto
{
    public Guid id { get;  private set; }
    public string fullName { get; private set; }
    public DateOnly dateOfBirth { get; private set; }

    public StudentDto(Guid id, string fullName, DateOnly dateOfBirth)
    {
        this.id = id;
        this.fullName = fullName;
        this.dateOfBirth = dateOfBirth;
    }
}}