using ManagementSystem.Domain.Entities;

public class AddExamToStudentHandler
{
    private readonly IStudentRepository _studentRepository;
    public AddExamToStudentHandler(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    public async Task Handle(AddExamToStudentCommand command)
    {
        var student = await _studentRepository.GetByIdAsync(command.StudentId.Value);
        if (student == null)
        {
            throw new Exception("Student not found");
        }

        var exam = new Exam(command.Subject, command.Type, command.Score);
        student.AddExam(exam);

        await _studentRepository.UpdateAsync(student);
    }
}