namespace Components.Exceptions;

public class NotSupportedException : Exception
{
    public NotSupportedException(string propertyName, string value) : base(
        $"{propertyName} with value '{value}' is not supported")
    {
    }
}