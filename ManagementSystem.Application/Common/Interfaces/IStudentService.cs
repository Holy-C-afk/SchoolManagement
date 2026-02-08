using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagementSystem.Application.Students;

public interface IStudentService
{
    Task<Guid> CreateAsync(CreateStudentCommand command);
    Task<StudentDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<StudentDto>> GetAllAsync();

}
