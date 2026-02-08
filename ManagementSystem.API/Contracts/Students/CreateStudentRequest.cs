namespace ManagementSystem.API.Models.Requests;

public record  CreateStudentRequest(
     string FirstName,
     string LastName,
     string Email,
     DateOnly BirthDate
);