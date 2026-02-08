namespace ManagementSystem.Domain.ValueObjects
{
    public sealed class ExamId
    {
        public Guid Value { get; }

        public ExamId(Guid value)
        {
            if (value == Guid.Empty)
                throw new  ArgumentException("ExamId cannot be empty");

            Value = value;
        }

        public static ExamId New() => new(Guid.NewGuid());
    }
}
