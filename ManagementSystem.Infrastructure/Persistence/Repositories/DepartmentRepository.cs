using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;
using ManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ManagementSystem.Infrastructure.Persistence.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ManagementDbContext _context;

    public DepartmentRepository(ManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .AsNoTracking() // Améliore les performances pour la lecture seule
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(DepartmentId id)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
    }

    public void Update(Department department)
    {
        _context.Departments.Update(department);
    }

    public void Delete(Department department)
    {
        _context.Departments.Remove(department);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}