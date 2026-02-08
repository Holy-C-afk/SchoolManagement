using System;

namespace ManagementSystem.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
// End of file: ManagementSystem.Domain/Exceptions/DomainException.cs