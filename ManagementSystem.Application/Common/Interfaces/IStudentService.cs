using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagementSystem.Application.Students;

public interface IStudentService
{
    Task<Guid> CreateAsync(CreateStudentCommand command);
    Task<StudentDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<StudentDto>> GetAllAsync();
    Task<(IReadOnlyList<StudentDto> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize,
        string? search = null);
    Task UpdateAsync(Guid id, UpdateStudentCommand command);
    Task DeleteAsync(Guid id);
    Task<IReadOnlyList<Guid>> BulkCreateAsync(IEnumerable<CreateStudentCommand> commands);
}
