public record CreateStudentCommand(
    string fullName,
    DateOnly BirthDate
);