using ManagementSystem.Application.Teachers.DTOs;
namespace ManagementSystem.Application.Common.Interfaces;

public interface ITeacherService
{
    Task<(IReadOnlyList<TeacherDto> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? search = null);
    Task<Guid> CreateAsync(string fullName, Guid departmentId);
    Task UpdateAsync(Guid id, string fullName, Guid departmentId);
    Task DeleteAsync(Guid id);
}