using Infrastructure.StronglyTypedIds;

namespace DataModel.Models;

/// <summary>
/// Data base model to include default columns such as Id and CreatedUtc
/// </summary>
/// <typeparam name="TId">Strongly typed id type</typeparam>
public abstract class BaseModel<TId> where TId : StronglyTypedIdBaseEntity
{
    public TId Id { get; } = StronglyTypedIdBaseEntity.New<TId>();

    public DateTime CreatedUtc { get; } = DateTime.UtcNow;
}