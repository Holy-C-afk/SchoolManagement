namespace ManagementSystem.API.Models.Requests;

public record UpdateStudentRequest(
     string FirstName,
     string LastName,
     DateOnly BirthDate
);