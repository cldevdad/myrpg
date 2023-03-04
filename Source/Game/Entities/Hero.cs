using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRpg.Engine.Components;
using MyRpg.Engine.Entities;
using MyRpg.Engine.Enums;

namespace MyRpg.Entities;

/// <summary>
/// A hero character that can move in four directions.
/// </summary>
public class Hero : DisplayEntity
{
    /// <summary>
    /// Initializes and returns a new instance of the Hero class.
    /// </summary>
    /// <param name="id">Unique identifier for the entity.</param>
    /// <param name="position">Screen position of the entity.</param>
    public Hero(string id, Vector2 position = default(Vector2))
        : base(id, position)
    {
        Components.Add(new Component("speed", typeof(float), 200f));
    }

    /// <summary>
    /// The value of the entity's speed component.
    /// </summary>
    public float Speed
    {
        get => Components.Find(c => c.Id == "speed")?.GetValue<float>() ?? 0f;
        set => Components.Find(c => c.Id == "speed")?.SetValue<float>(value);
    }

    /// <inheritdoc />
    public override void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        AddTexturesAndSet(new List<string>() { "hero", "hero_back", "hero_left", "hero_right" }, "hero");
        base.LoadContent(contentManager, graphicsDevice);
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime, KeyboardState? keyboardState = null)
    {
        if ((keyboardState?.IsKeyDown(Keys.Up) ?? false) || (keyboardState?.IsKeyDown(Keys.W) ?? false))
        {
            MoveDirection(Direction.UP, gameTime);
        }

        if ((keyboardState?.IsKeyDown(Keys.Down) ?? false) || (keyboardState?.IsKeyDown(Keys.S) ?? false))
        {
            MoveDirection(Direction.DOWN, gameTime);
        }

        if ((keyboardState?.IsKeyDown(Keys.Left) ?? false) || (keyboardState?.IsKeyDown(Keys.A) ?? false))
        {
            MoveDirection(Direction.LEFT, gameTime);
        }

        if ((keyboardState?.IsKeyDown(Keys.Right) ?? false) || (keyboardState?.IsKeyDown(Keys.D) ?? false))
        {
            MoveDirection(Direction.RIGHT, gameTime);
        }
    }

    /// <summary>
    /// Move the character in a direction.
    /// </summary>
    /// <param name="direction">Direction to move character.</param>
    /// <param name="gameTime">Elapsed time since the last update.</param>
    private void MoveDirection(Direction direction, GameTime gameTime)
    {
        var position = Position;

        switch (direction)
        {
            case Direction.UP:
                SetTexture("hero_back");
                position.Y -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                break;
            case Direction.DOWN:
                SetTexture("hero");
                position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                break;
            case Direction.LEFT:
                SetTexture("hero_left");
                position.X -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                break;
            case Direction.RIGHT:
                SetTexture("hero_right");
                position.X += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                break;
            default:
                break;
        }

        Position = position;
    }
}
