using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MyRpg.Api;

/// <summary>
/// Shape of a distinct state of game entity presentation.
/// </summary>
internal interface IScene
{
    /// <summary>
    /// Gets or sets unique identifier.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether whether or not the scene will update or render.
    /// </summary>
    bool Active { get; set; }

    /// <summary>
    /// Gets collection of all entities on the scene.
    /// </summary>
    List<IEntity> Entities { get; }

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
    /// <param name="transformMatrix">Transform matrix with translation, rotation, and scale information.</param>
    void Draw(Matrix? transformMatrix = null);
}
