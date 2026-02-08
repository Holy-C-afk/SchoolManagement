public sealed class TeacherId
{
    public Guid Value { get; }

    public TeacherId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("TeacherId cannot be empty");

        Value = value;
    }
}