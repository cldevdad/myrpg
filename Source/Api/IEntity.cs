using System.Collections.Generic;

namespace MyRpg.Api;

/// <summary>
/// Shape of an entity that can be added to a scene.
/// </summary>
public interface IEntity : IObjectWithId
{
    /// <summary>
    /// Type of the entity.
    /// </summary>
    EntityType Type { get; }

    /// <summary>
    /// Components of the entity.
    /// </summary>
    List<IComponent> Components { get; set; }
}
