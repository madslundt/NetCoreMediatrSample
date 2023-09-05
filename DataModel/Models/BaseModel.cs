using Infrastructure.StronglyTypedIds;

namespace DataModel.Models;

/// <summary>
///     Data base model to include default columns such as Id and CreatedUtc
/// </summary>
/// <typeparam name="TId">Strongly typed id type</typeparam>
public abstract class BaseModel<TId> where TId : StronglyTypedIdBaseEntity<TId>
{
    public TId Id { get; } = StronglyTypedIdBaseEntity<TId>.New();

    public DateTime CreatedUtc { get; } = DateTime.UtcNow;
}