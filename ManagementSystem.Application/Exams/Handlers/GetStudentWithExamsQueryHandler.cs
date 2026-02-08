using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

public class GetStudentWithExamsQueryHandler
{
    private readonly IStudentRepository _studentRepository;
    public GetStudentWithExamsQueryHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<StudentWithExamsDto> Handle(GetStudentWithExamsQuery query)
    {
        var student = await _studentRepository.GetByIdAsync(new StudentId(query.StudentId));
        if (student == null)
        {
            throw new Exception("Student not found");
        }
        var dto = new StudentWithExamsDto
        {
            Id = student.Id.Value,
            FullName = student.FullName,
            BirthDate = student.BirthDate,

        };
        foreach (var exam in student.Exams)
        {
            dto.Exams.Add(new ExamDto
            {
                Subject = exam.Subject,
                Type = exam.Type.ToString(),
                Score = exam.Score.Value
            });
            
        }
        return dto;
    }
}