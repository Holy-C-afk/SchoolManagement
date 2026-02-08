using ManagementSystem.Domain.ValueObjects;

public record AddExamToStudentCommand(StudentId StudentId, string Subject, ExamType Type, Score Score);
