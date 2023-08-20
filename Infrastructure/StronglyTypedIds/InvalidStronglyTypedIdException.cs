namespace Infrastructure.StronglyTypedIds;

public class InvalidStronglyTypedIdException : Exception
{
    public InvalidStronglyTypedIdException(string message) : base(message)
    {
    }
}