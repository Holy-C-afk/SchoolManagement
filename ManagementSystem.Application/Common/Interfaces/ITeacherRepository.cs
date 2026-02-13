using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Application.Common.Interfaces;

public interface ITeacherRepository
{
    // Récupère un enseignant par son ID (Value Object)
    Task<Teacher?> GetByIdAsync(TeacherId id);

    // Version paginée avec filtre de recherche
    Task<(IReadOnlyList<Teacher> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? search = null);

    // Méthodes de persistance
    Task AddAsync(Teacher teacher);
    void Update(Teacher teacher);
    void Delete(Teacher teacher);
    
    // Sauvegarde les changements
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}