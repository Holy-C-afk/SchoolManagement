using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

public interface IStudentRepository
{
    Task AddAsync(Student student);
    Task<Student?> GetByIdAsync(StudentId id);
    Task UpdateAsync(Student student);
}