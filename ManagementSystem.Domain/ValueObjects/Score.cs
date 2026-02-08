using ManagementSystem.Domain.Exceptions;

namespace ManagementSystem.Domain.ValueObjects
{
    public sealed class Score
    {
        public decimal Value { get; }

        public Score(decimal value)
        {
            if (value < 0 || value > 20)
                throw new DomainException("Score must be between 0 and 20");

            Value = value;
        }
    }
}
