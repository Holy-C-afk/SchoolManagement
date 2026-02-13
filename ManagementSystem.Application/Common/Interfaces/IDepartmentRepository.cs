using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;
namespace ManagementSystem.Application.Common.Interfaces;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(DepartmentId id);
    Task AddAsync(Department department);
    void Update(Department department);
    void Delete(Department department);
    Task SaveChangesAsync();
}