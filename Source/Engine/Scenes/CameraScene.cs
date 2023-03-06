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
    /// Initializes a new camera scene and resolves any dependencies the scene needs.
    /// </summary>
    /// <param name="Id">Unique identifier for the scene.</param>
    protected CameraScene(string? id = null)
        : base(id) { }

    /// <inheritdoc />
    public override void Initialize() { }

    /// <inheritdoc />
    public override void LoadContent()
    {
        var camera = new OverheadCamera(this.GraphicsDevice.Viewport, new Vector2(Width / 2, Height / 2));
        Entities.Add("camera", camera);

        foreach (var entity in Entities.Where(e => e.Value.GetType().IsAssignableTo(typeof(IContentEntity))))
        {
            (entity.Value as IContentEntity)?.LoadContent(this.ContentManager, this.GraphicsDevice);
        }
    }

    /// <inheritdoc />
    public override void Draw(Matrix? transformMatrix = null)
    {
        this.SpriteBatch.Begin(transformMatrix: this.Camera.Transform);
        if (Active)
        {
            foreach (var entity in Entities.Where(e => e.Value.GetType().IsAssignableTo(typeof(IDrawableEntity))))
            {
                (entity.Value as IDrawableEntity)?.Draw(this.SpriteBatch, this.Camera.Transform);
            }
        }

        this.SpriteBatch.End();
    }

    /// <summary>
    /// The value of the entity's camera component.
    /// </summary>
    private OverheadCamera Camera
    {
        get => (OverheadCamera)Entities["camera"];
        set => Entities["camera"] = value;
    }
}
