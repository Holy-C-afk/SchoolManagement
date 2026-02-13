using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Infrastructure.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly ManagementDbContext _context;

    // 1. Ajout du constructeur (indispensable pour accéder à la DB)
    public TeacherRepository(ManagementDbContext context)
    {
        _context = context;
    }

    // 2. Récupération par ID avec le Value Object TeacherId
    public async Task<Teacher?> GetByIdAsync(TeacherId id)
    {
        return await _context.Teachers
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    // 3. Ta méthode GetPaged modifiée et optimisée
    public async Task<(IReadOnlyList<Teacher> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? search = null)
    {
        var query = _context.Teachers
            .Include(t => t.Department)
            .AsNoTracking() // Optimisation pour la lecture seule
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Recherche insensible à la casse (selon collation DB) sur le Nom ou le Département
            query = query.Where(t => 
                EF.Functions.Like(t.FullName, $"%{search}%") || 
                EF.Functions.Like(t.Department.Name, $"%{search}%"));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(t => t.FullName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    // 4. Ajout des méthodes de persistance manquantes
    public async Task AddAsync(Teacher teacher)
    {
        await _context.Teachers.AddAsync(teacher);
    }

    public void Update(Teacher teacher)
    {
        _context.Teachers.Update(teacher);
    }

    public void Delete(Teacher teacher)
    {
        _context.Teachers.Remove(teacher);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}