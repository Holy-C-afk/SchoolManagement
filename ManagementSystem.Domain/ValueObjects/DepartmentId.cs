namespace ManagementSystem.Domain.ValueObjects
{
    public sealed class DepartmentId
    {
        public Guid Value {get;}
        public DepartmentId(Guid value)
        {/*
            if (Value == Guid.Empty)
                throw new ArgumentException("DepartmentId cannot be empty");
*/
            this.Value = value;
        }
    }
}