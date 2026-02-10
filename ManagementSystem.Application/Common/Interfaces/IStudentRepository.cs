using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

public interface IStudentRepository
{
    Task AddAsync(Student student);
    Task AddRangeAsync(IEnumerable<Student> students);
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task UpdateAsync(Student student);
    Task DeleteAsync(Guid id);
}