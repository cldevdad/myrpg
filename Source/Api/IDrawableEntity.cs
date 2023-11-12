using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyRpg.Enums;

namespace MyRpg.Api;

/// <summary>
/// Shape of an entity with drawable components.
/// </summary>
public interface IDrawableEntity : IEntity
{
    /// <summary>
    /// Gets or sets z-order layer when drawing the entity.
    /// </summary>
    DrawLayer Layer { get; set; }

    /// <summary>
    /// Gets the entity's bounding rectangle.
    /// </summary>
    /// <returns>The entity's bounding rectangle.</returns>
    Rectangle GetBoundingRect();

    /// <summary>
    /// Draws all drawable components attached to the entity.
    /// </summary>
    /// <param name="spriteBatch">Microsoft.Xna.Framework.Graphics.SpriteBatch used to draw with.</param>
    /// <param name="transformMatrix">Transform matrix with translation, rotation, and scale information.</param>
    void Draw(SpriteBatch spriteBatch, Matrix? transformMatrix = null);

    /// <summary>
    /// Gets or sets a value indicating whether whether or not the entity has a custom renderer. If true
    /// the entity will not be drawn by the scene's default renderer.
    /// </summary>
    bool CustomRenderer { get; set; }
}
