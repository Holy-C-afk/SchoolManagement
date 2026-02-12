namespace ManagementSystem.Application.Common.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateStudentsPdf(IEnumerable<StudentDto> students);
    }
}
