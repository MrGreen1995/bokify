namespace Bookify.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; init; }

    public Entity(Guid id)
    {
        Id = id;
    }
}