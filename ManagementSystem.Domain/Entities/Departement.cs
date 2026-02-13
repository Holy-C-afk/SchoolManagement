using ManagementSystem.Domain.ValueObjects;
namespace ManagementSystem.Domain.Entities
{
    public sealed class Department 
    {
        public DepartmentId Id { get; private set; }
        public string Name { get; private set; }
        private Department() { } // Pour EF Core
        public Department(string name)
        {
            Id = new DepartmentId(Guid.NewGuid());
            Name = name;
        }
    }
}
