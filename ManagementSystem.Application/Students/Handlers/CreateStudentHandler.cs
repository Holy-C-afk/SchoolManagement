using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

public class CreateStudentHandler
{
    private readonly IStudentRepository _studentRepository;

    public CreateStudentHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<StudentId> Handle(CreateStudentCommand command)
    {
        var student  = new Student(command.fullName, command.BirthDate);
        await _studentRepository.AddAsync(student);
        return student.Id;
    }
}