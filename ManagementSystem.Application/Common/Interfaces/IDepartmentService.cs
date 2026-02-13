using ManagementSystem.Application.Departments.DTOs;

namespace ManagementSystem.Application.Common.Interfaces;

public interface IDepartmentService
{
    // Récupérer tous les départements (utile pour les listes déroulantes dans le front)
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    
    // Récupérer un département par son ID
    Task<DepartmentDto?> GetByIdAsync(Guid id);
    
    // Créer un nouveau département
    Task<Guid> CreateAsync(string name);
    
    // Mettre à jour un département existant
    Task UpdateAsync(Guid id, string name);
    
    // Supprimer un département
    Task DeleteAsync(Guid id);
}