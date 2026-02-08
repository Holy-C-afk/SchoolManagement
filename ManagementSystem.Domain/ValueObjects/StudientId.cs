namespace ManagementSystem.Domain.ValueObjects
{
    public sealed class StudentId
    {
        public Guid Value { get; }

        public StudentId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("StudentId cannot be empty");

            Value = value;
        }

        public static StudentId New() => new(Guid.NewGuid());
    }
}
