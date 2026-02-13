namespace ManagementSystem.API.Contracts.Teachers;

public record UpdateRequestTeacher(
    string FullName,
    Guid DepartmentId
);