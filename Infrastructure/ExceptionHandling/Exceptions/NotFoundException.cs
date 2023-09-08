using Infrastructure.StronglyTypedIds;

namespace Infrastructure.ExceptionHandling.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string propertyName, string id) : base(PropertyIdMessage(propertyName, id))
    {
    }

    public NotFoundException(string propertyName, StronglyTypedIdBaseEntity id) : base(
        PropertyIdMessage(propertyName, id.ToString()))
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    private static string PropertyIdMessage(string propertyName, string id)
    {
        return $"{propertyName} with id '{id}' was not found";
    }
}