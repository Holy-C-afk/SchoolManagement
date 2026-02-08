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

        public async Task<Student?> GetByIdAsync(StudentId id)
        {
            return await _context.Students
                .Include(s => s.Exams)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
