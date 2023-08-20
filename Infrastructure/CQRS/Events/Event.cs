namespace Infrastructure.CQRS.Events;

public abstract class Event : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreatedUtc { get; } = DateTime.UtcNow;
}