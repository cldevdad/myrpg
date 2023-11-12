using System.Linq;
using Microsoft.Xna.Framework;
using MyRpg.Api;

namespace MyRpg.Engine.Scenes;

/// <summary>
/// A scene that has a camera.
/// </summary>
internal abstract class CameraScene : GameScene
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CameraScene"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the scene.</param>
    protected CameraScene(string? id = null)
        : base(id) { }

    /// <inheritdoc />
    public override void Initialize() { }

    /// <inheritdoc />
    public override void LoadContent()
    {
        var camera = new OverheadCamera(this.GraphicsDevice.Viewport, new Vector2(Width / 2, Height / 2));
        Entities.Add(camera);

        foreach (var entity in Entities.Where(e => e.GetType().IsAssignableTo(typeof(IContentEntity))))
        {
            (entity as IContentEntity)?.LoadContent(this.ContentManager, this.GraphicsDevice);
        }
    }
}
