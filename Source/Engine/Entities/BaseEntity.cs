using System.Collections.Generic;
using MyRpg.Api;

namespace MyRpg.Engine.Entities;

/// <summary>
/// Base entity that all other game entities derive from.
/// </summary>
public abstract class BaseEntity : IEntity, IObjectWithId
{
    /// <inheritdoc />
    public string Id { get; set; }

    /// <inheritdoc />
    public virtual EntityType Type { get; } = EntityType.UNKNOWN;

    /// <inheritdoc />
    public List<IComponent> Components { get; set; } = new List<IComponent>();

    /// <summary>
    /// Constructs and returns a new entity with an id.
    /// </summary>
    /// <param name="id"></param>
    protected BaseEntity(string id)
    {
        Id = id;
    }
}
