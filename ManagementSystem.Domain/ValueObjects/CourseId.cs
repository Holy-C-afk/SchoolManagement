public sealed class CourseId
{
    public Guid Value { get; }

    public CourseId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("CourseId cannot be empty");

        Value = value;
    }

    // Ajoute ceci pour Entity Framework
    private CourseId() { } 
}