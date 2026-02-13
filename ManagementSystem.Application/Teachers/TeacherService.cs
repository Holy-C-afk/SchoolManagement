using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Teachers.DTOs;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

namespace ManagementSystem.Application.Teachers;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;

    public TeacherService(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<(IReadOnlyList<TeacherDto> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? search = null)
    {
        var (teachers, totalCount) = await _teacherRepository.GetPagedAsync(pageNumber, pageSize, search);

        var dtos = teachers.Select(t => new TeacherDto(
            t.Id.Value,
            t.FullName,
            t.Department?.Name ?? "Aucun département"
        )).ToList();

        return (dtos, totalCount);
    }

    public async Task<Guid> CreateAsync(string fullName, Guid departmentId)
    {
        // Utilisation du constructeur public : public Teacher(string fullName, DepartmentId d)
        // Note : Ton constructeur génère déjà l'ID à l'intérieur, donc on passe juste les 2 arguments
        var teacher = new Teacher(
            fullName, 
            new DepartmentId(departmentId)
        );

        await _teacherRepository.AddAsync(teacher);
        await _teacherRepository.SaveChangesAsync();

        return teacher.Id.Value;
    }

    public async Task UpdateAsync(Guid id, string fullName, Guid departmentId)
    {
        var teacher = await _teacherRepository.GetByIdAsync(new TeacherId(id));
        
        if (teacher == null) return;

        // Problème : Ton entité n'a pas de méthode Update ni de setters publics.
        // Comme tu as des "private set", tu DOIS ajouter une méthode dans Teacher.cs
        // Sinon, le service ne pourra JAMAIS modifier un professeur existant.
        
        // Appelle une méthode que tu vas ajouter dans Teacher.cs (voir ci-dessous)
        teacher.UpdateDetails(fullName, new DepartmentId(departmentId));

        _teacherRepository.Update(teacher);
        await _teacherRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(new TeacherId(id));
        if (teacher != null)
        {
            _teacherRepository.Delete(teacher);
            await _teacherRepository.SaveChangesAsync();
        }
    }
}