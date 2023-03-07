using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MyRpg.Api;

/// <summary>
/// Shape of an entity with content resource components.
/// </summary>
public interface IContentEntity : IEntity
{
    /// <summary>
    /// Root path to content the entity uses.
    /// </summary>
    string ContentRoot { get; set; }

    /// <summary>
    /// Load all content required by the entity.
    /// </summary>
    /// <param name="contentManager">The game's Microsoft.Xna.Framework.Content.ContentManager dependency.</param>
    /// <param name="graphicsDevice">The game's Microsoft.Xna.Framework.Graphics.GraphicsDevice dependency.</param>
    void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null);
}
