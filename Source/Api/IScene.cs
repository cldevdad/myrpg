using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MyRpg.Api;

/// <summary>
/// Shape of a distinct state of game entity presentation.
/// </summary>
internal interface IScene
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Whether or not the scene will update or render.
    /// </summary>
    bool Active { get; set; }

    /// <summary>
    /// Collection of string keys and Entity values for all entities on the scene.
    /// </summary>
    Dictionary<string, IEntity> Entities { get; }

    /// <summary>
    /// Initialize the scene before loading content.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Load all content resources required by the scene.
    /// </summary>
    void LoadContent();

    /// <summary>
    /// Unload all loaded content.
    /// </summary>
    void UnloadContent();

    /// <summary>
    /// Updates all updatable entities attached to the scene.
    /// </summary>
    /// <param name="gameTime">Elapsed time since the last update.</param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Draws all drawable entities attached to the scene.
    /// </summary>
    void Draw(Matrix? transformMatrix = null);
}
