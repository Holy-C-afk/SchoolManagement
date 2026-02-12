using Microsoft.EntityFrameworkCore;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

namespace ManagementSystem.Infrastructure.Persistence.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ManagementDbContext _context;

        public StudentRepository(ManagementDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Student> students)
        {
            _context.Students.AddRange(students);
            await _context.SaveChangesAsync();
        }
        public async Task<(IReadOnlyList<Student> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? search = null)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    EF.Functions.Like(s.FullName, $"%{search}%"));
            }

            var totalCount = await query.CountAsync();

            var Items = await query
                .OrderBy(s => s.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (Items, totalCount);
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            var studentId = new StudentId(id);

            // Pas de Include ici pour éviter les doublons dus aux jointures
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task UpdateAsync(Student student)
        {
            // Pour éviter les conflits de tracking sur le PUT :
            var trackedEntity = _context.Students.Local.FirstOrDefault(s => s.Id.Value == student.Id.Value);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Student>> GetAllAsync()
        {
            return await _context.Students
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var studentId = new StudentId(id);
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student is null)
            {
                return;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}