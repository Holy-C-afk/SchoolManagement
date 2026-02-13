using ManagementSystem.Domain.ValueObjects; // Pour reconnaître StudentId
using ManagementSystem.Application.Common.Interfaces; // Pour reconnaître IStudentRepository
using ManagementSystem.Application.Students.DTOs; // Pour reconnaître StudentDto
public class GetStudentByIdhandler
{
    private readonly IStudentRepository _studentRepository;

    public GetStudentByIdhandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    public async Task<StudentDto?> Handle(GetStudentByIdQuery query)
    {
        var student = await _studentRepository.GetByIdAsync(query.Id.Value);
        if (student == null)
        {
            return null;
        }

        return new StudentDto(student.Id.Value, student.FullName, student.BirthDate);
    }
}