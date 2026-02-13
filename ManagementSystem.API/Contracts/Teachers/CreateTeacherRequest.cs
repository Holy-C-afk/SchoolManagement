namespace ManagementSystem.API.Contracts.Teachers;

// Requête pour la création d'un enseignant
public record CreateTeacherRequest(
    string FullName,
    Guid DepartmentId
);

// Requête pour la mise à jour d'un enseignant
public record UpdateTeacherRequest(
    string FullName,
    Guid DepartmentId
);