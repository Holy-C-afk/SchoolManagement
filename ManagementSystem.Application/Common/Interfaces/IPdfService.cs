using ManagementSystem.Application.Students.DTOs; 
using ManagementSystem.Application.Teachers.DTOs;
namespace ManagementSystem.Application.Common.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateStudentsPdf(IEnumerable<StudentDto> students);
        byte[] GenerateTeachersPdf(IEnumerable<TeacherDto> teachers);
    }
}
