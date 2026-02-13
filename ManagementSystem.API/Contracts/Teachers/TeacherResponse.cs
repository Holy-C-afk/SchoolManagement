namespace ManagementSystem.API.Models.Responses;

public sealed class TeacherResponse
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = null!;
    
    // On renvoie les infos du département lié
    public Guid DepartmentId { get; init; }
    public string DepartmentName { get; init; } = null!;
}