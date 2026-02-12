using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;

namespace ManagementSystem.Application.Students
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateAsync(CreateStudentCommand command)
        {
            var student = new Student(command.fullName, command.BirthDate);

            await _repository.AddAsync(student);
            return student.Id.Value;
        }

        public async Task<StudentDto?> GetByIdAsync(Guid id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student is null) return null;

            // On utilise le constructeur avec les noms de variables exacts
            return new StudentDto(
                student.Id.Value, 
                student.FullName, 
                student.BirthDate
            );
        }

        public async Task<IReadOnlyList<StudentDto>> GetAllAsync()
        {
            var students = await _repository.GetAllAsync();
    
            return students.Select(s => new StudentDto(
                s.Id.Value, 
                s.FullName, 
                s.BirthDate
            )).ToList();
        }
        public async Task<(IReadOnlyList<StudentDto> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, 
            int pageSize,
            string? search = null) // Le paramètre arrive bien ici...
        {
            // ...mais il faut l'envoyer au repository !
            var (students, totalCount) = await _repository.GetPagedAsync(pageNumber, pageSize, search);

            var items = students
                .Select(s => new StudentDto(s.Id.Value, s.FullName, s.BirthDate))
                .ToList();

            return (items, totalCount);
}

        public async Task UpdateAsync(Guid id, UpdateStudentCommand command)
        {
            var student = await _repository.GetByIdAsync(Guid.Parse(id.ToString()));
            if (student is null) throw new Exception("Student not found");

            student.UpdateName($"{command.FirstName} {command.LastName}");
            student.UpdateBirthDate(command.BirthDate);

            await _repository.UpdateAsync(student);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IReadOnlyList<Guid>> BulkCreateAsync(IEnumerable<CreateStudentCommand> commands)
        {
            var students = commands
                .Select(c => new Student(c.fullName, c.BirthDate))
                .ToList();

            await _repository.AddRangeAsync(students);

            return students.Select(s => s.Id.Value).ToList();
        }
    
    }
}
