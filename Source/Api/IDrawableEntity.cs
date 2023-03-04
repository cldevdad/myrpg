using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyRpg.Api;

/// <summary>
/// Shape of an entity with drawable components.
/// </summary>
public interface IDrawableEntity : IEntity
{
    /// <summary>
    /// Get the entity's bounding rectangle.
    /// </summary>
    /// <returns>The entity's bounding rectangle.</returns>
    Rectangle GetBoundingRect();

    /// <summary>
    /// Draws all drawable components attached to the entity.
    /// </summary>
    /// <param name="spriteBatch">Microsoft.Xna.Framework.Graphics.SpriteBatch used to draw with.</param>
    void Draw(SpriteBatch spriteBatch, Matrix? transformMatrix = null);
}
