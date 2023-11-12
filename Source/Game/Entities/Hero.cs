using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRpg.Engine.Components;
using MyRpg.Engine.Entities;
using MyRpg.Engine.Enums;
using MyRpg.Engine.Utilities;
using MyRpg.Enums;

namespace MyRpg.Entities;

/// <summary>
/// A hero character that can walk and sprint in four directions.
/// </summary>
public class Hero : AnimatedDisplayEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Hero"/> class.
    /// </summary>
    /// <param name="id">Unique identifier for the entity.</param>
    /// <param name="frameSize">Animation frame size.</param>
    /// <param name="position">Screen position of the entity.</param>
    public Hero(string id, Size frameSize, Vector2 position = default(Vector2))
        : base(id, frameSize, position)
    {
        Components.Add(new Component("speed", typeof(float), 200f));
        AnimationDefinition.FrameDuration = Speed;
        AnimationDefinition.CycleDirections = true;
        AnimationRow = AnimationTextureRowIndex.WALK_DOWN;
        CustomRenderer = true;
    }

    /// <summary>
    /// Gets or sets the value of the entity's speed component.
    /// </summary>
    public float Speed
    {
        get => Components.Find(c => c.Id == "speed")?.GetValue<float>() ?? 0f;
        set => Components.Find(c => c.Id == "speed")?.SetValue<float>(value);
    }

    /// <inheritdoc />
    public override void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        ContentRoot = "Characters";
        Layer = DrawLayer.ACTOR_1;

        AddTextureAndSet($"{ContentRoot}/hero");

        base.LoadContent(contentManager, graphicsDevice);
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime, KeyboardState? keyboardState = null)
    {
        base.Update(gameTime, keyboardState);

        PauseAnimation();

        // Hero movement
        var wasSprinting = _sprinting;
        _sprinting =
            (keyboardState?.IsKeyDown(Keys.RightShift) ?? false) || (keyboardState?.IsKeyDown(Keys.LeftShift) ?? false);

        if (_sprinting != wasSprinting)
        {
            AnimationDefinition.FrameDuration = _sprinting ? Speed / 2 : Speed;
        }

        if ((keyboardState?.IsKeyDown(Keys.Up) ?? false) || (keyboardState?.IsKeyDown(Keys.W) ?? false))
        {
            MoveInDirection(Direction.UP, gameTime);
        }

        if ((keyboardState?.IsKeyDown(Keys.Down) ?? false) || (keyboardState?.IsKeyDown(Keys.S) ?? false))
        {
            MoveInDirection(Direction.DOWN, gameTime);
        }

        if ((keyboardState?.IsKeyDown(Keys.Left) ?? false) || (keyboardState?.IsKeyDown(Keys.A) ?? false))
        {
            MoveInDirection(Direction.LEFT, gameTime);
        }

        if ((keyboardState?.IsKeyDown(Keys.Right) ?? false) || (keyboardState?.IsKeyDown(Keys.D) ?? false))
        {
            MoveInDirection(Direction.RIGHT, gameTime);
        }
    }

    private bool _sprinting = false;

    /// <summary>
    /// Move the character in a direction.
    /// </summary>
    /// <param name="direction">Direction to move in.</param>
    /// <param name="gameTime">Elapsed time since the last update.</param>
    private void MoveInDirection(Direction direction, GameTime gameTime)
    {
        PlayAnimation(loop: true, cycleDirections: true);

        var position = Position;
        var speed = _sprinting ? Speed * 2 : Speed;

        switch (direction)
        {
            case Direction.UP:
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimationRow = AnimationTextureRowIndex.WALK_UP;
                break;
            case Direction.DOWN:
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimationRow = AnimationTextureRowIndex.WALK_DOWN;
                break;
            case Direction.LEFT:
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimationRow = AnimationTextureRowIndex.WALK_LEFT;
                break;
            case Direction.RIGHT:
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimationRow = AnimationTextureRowIndex.WALK_RIGHT;
                break;
            default:
                break;
        }

        Position = position;
    }
}
