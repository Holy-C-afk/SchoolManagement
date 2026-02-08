public sealed class DepartmentId
{
    public Guid Value {get;}
    public DepartmentId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("DepartmentId cannot be empty");

        Value = value;
    }
}