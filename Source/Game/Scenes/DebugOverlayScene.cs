using Microsoft.Xna.Framework;
using MyRpg.Engine.Scenes;
using MyRpg.Entities;

namespace MyRpg.Scenes;

/// <summary>
/// The debug information overlay scene.
/// </summary>
internal class DebugOverlayScene : GameScene
{
    /// <summary>
    /// Initializes and returns a new instance of the DebugOverlayScene class.
    /// </summary>
    /// <param name="id">Unique identifier for the scene.</param>
    public DebugOverlayScene(string? id = null)
        : base(id) { }

    /// <inheritdoc />
    public override void Initialize()
    {
        Entities.Add("fpsCounter", new FpsCounter("fps-counter", new Vector2(5, Height - 20)));
    }
}
