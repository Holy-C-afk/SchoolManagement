using ManagementSystem.Domain.Exceptions;
using ManagementSystem.Domain.ValueObjects;

namespace ManagementSystem.Domain.Entities
{
    public class Exam
    {
        public ExamId Id { get; private set; }
        public string Subject { get; private set; }
        public ExamType Type { get; private set; }
        public Score Score { get; private set; }

        private Exam() { } // for Orm

        public Exam(string subject, ExamType type, Score score)
        {
            Id = ExamId.New();
            Subject = subject ?? throw new DomainException(nameof(subject));
            Type = type;
            Score = score ?? throw new DomainException(nameof(score));
        }
    }
}
