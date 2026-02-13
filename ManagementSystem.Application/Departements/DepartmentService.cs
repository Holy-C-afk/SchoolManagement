using ManagementSystem.Application.Common.Interfaces; // C'est ici que doit être IDepartmentRepository
using ManagementSystem.Application.Departments.DTOs;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

namespace ManagementSystem.Application.Departments;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        
        return departments.Select(d => new DepartmentDto(
            d.Id.Value,
            d.Name
        ));
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(new DepartmentId(id));
        
        if (department == null) return null;

        return new DepartmentDto(department.Id.Value, department.Name);
    }

    public async Task<Guid> CreateAsync(string name)
    {
        // Utilisation du constructeur de ton entité Department
        var department = new Department(name);

        await _departmentRepository.AddAsync(department);
        await _departmentRepository.SaveChangesAsync();

        return department.Id.Value;
    }

    public async Task UpdateAsync(Guid id, string name)
    {
        var department = await _departmentRepository.GetByIdAsync(new DepartmentId(id));
        
        if (department == null) return;

        // On appelle la méthode de mise à jour de l'entité
        department.UpdateName(name);

        _departmentRepository.Update(department);
        await _departmentRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(new DepartmentId(id));
        
        if (department != null)
        {
            _departmentRepository.Delete(department);
            await _departmentRepository.SaveChangesAsync();
        }
    }
    
}