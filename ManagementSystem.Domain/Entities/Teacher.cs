using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.ValueObjects;
namespace ManagementSystem.Domain.Entities;
public sealed class Teacher 
{
    public TeacherId Id { get; private set; }
    public string FullName { get; private set; }
    public DepartmentId DepartmentId { get; private set; }
    public Department Department { get; private set; }
    private Teacher() { } // Pour EF Core

    public Teacher(string fullName, DepartmentId d)
    {
        Id = new TeacherId(Guid.NewGuid());
        FullName = fullName;
        DepartmentId = d;
    }
    public void UpdateDetails(string fullName, DepartmentId d)
    {
        FullName = fullName;
        DepartmentId = d;
    }
}
