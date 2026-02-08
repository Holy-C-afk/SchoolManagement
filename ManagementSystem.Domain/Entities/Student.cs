using ManagementSystem.Domain.ValueObjects;
using ManagementSystem.Domain.Exceptions;
namespace ManagementSystem.Domain.Entities;

public class Student
{
    public StudentId Id { get; private set; }
    public string FullName { get; private set; }
    
    public DateOnly BirthDate { get; private set; }
    private readonly List<Exam> _Exams = new();
    public IReadOnlyCollection<Exam> Exams => _Exams.AsReadOnly();
    private Student() { } // for Orm
    public Student(string fullName, DateOnly birthDate)
    {
        this.Id = StudentId.New();
        if(string.IsNullOrEmpty(fullName))
            throw new DomainException("FullName cannot be null or empty");
        if(birthDate > DateOnly.FromDateTime(DateTime.Now))
            throw new DomainException("BirthDate cannot be in the future");
        this.FullName = fullName;
        this.BirthDate = birthDate;
    }

    public void AddExam(Exam exam)
    {
        if(exam == null)
            throw new DomainException("Exam cannot be null");
        _Exams.Add(exam);
    }
}