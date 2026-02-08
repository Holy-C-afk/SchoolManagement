public class StudentWithExamsDto
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public DateOnly BirthDate { get; set; }
    public List<ExamDto> Exams { get; set; } = new();
}
