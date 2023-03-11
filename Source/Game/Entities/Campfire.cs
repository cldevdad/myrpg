using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyRpg.Engine.Entities;
using MyRpg.Engine.Utilities;

namespace MyRpg.Entities;

/// <summary>
/// A campfire that steadily burns bright.
/// </summary>
public class Campfire : AnimatedDisplayEntity
{
    /// <summary>
    /// Initializes and returns a new instance of the Campfire class.
    /// </summary>
    /// <param name="id">Unique identifier for the entity.</param>
    /// <param name="frameSize">Animation frame size.</param>
    /// <param name="position">Screen position of the entity.</param>
    public Campfire(string id, Size frameSize, Vector2 position = default(Vector2))
        : base(id, frameSize, position)
    {
        AnimationDefinition.FrameDuration = 150f;
        AnimationDefinition.CycleDirections = true;
        AnimationDefinition.Loop = true;
        AnimationRow = 0;
    }

    /// <inheritdoc />
    public override void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        ContentRoot = "Map Objects";

        AddTextureAndSet($"{ContentRoot}/Campfire");
        PlayAnimation(loop: true, cycleDirections: true);

        base.LoadContent(contentManager, graphicsDevice);
    }
}
