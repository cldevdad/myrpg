using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRpg.Engine.Definitions;
using MyRpg.Engine.Enums;
using MyRpg.Engine.Utilities;

namespace MyRpg.Engine.Entities;

/// <summary>
/// A display entity that animates using frames in its spritesheet texture.
/// </summary>
public abstract class AnimatedDisplayEntity : DisplayEntity
{
    /// <inheritdoc />
    public override EntityType Type
    {
        get => EntityType.ANIMATION;
    }

    /// <summary>
    /// The AnimatedDisplayEntityDefinition for the entity.
    /// </summary>
    public AnimatedDisplayEntityDefinition AnimationDefinition { get; set; }

    /// <summary>
    /// Whether or not the animation is currently playing.
    /// </summary>
    public bool Playing { get; set; }

    /// <summary>
    /// Index of the row of animation frames in the entity's texture.
    /// </summary>
    public AnimationTextureRowIndex AnimationRow
    {
        get => _currentRowIndex;
        set
        {
            if (_currentRowIndex != value)
            {
                _currentRowIndex = value;

                // Change the source rect's y position immediately for the next draw
                _sourceRect.Y = (int)AnimationRow * (int)AnimationDefinition.FrameSize.Height;
            }
        }
    }

    /// <summary>
    /// Initialize and return a new Animated Display Entity.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="frameSize"></param>
    /// <param name="position">Optional position of the entity.</param>
    protected AnimatedDisplayEntity(string id, Size frameSize, Vector2 position = default(Vector2))
        : base(id, position)
    {
        AnimationDefinition = new AnimatedDisplayEntityDefinition()
        {
            FrameSize = frameSize,
            FrameDuration = 0f,
            Loop = false,
            CycleDirections = false
        };

        Playing = false;
        _animationDirection = AnimationDirection.LEFT_TO_RIGHT;
        _currentFrame = 0;

        _sourceRect = new Rectangle(
            _currentFrame * (int)AnimationDefinition.FrameSize.Width,
            (int)AnimationRow,
            (int)AnimationDefinition.FrameSize.Width,
            (int)AnimationDefinition.FrameSize.Height
        );
    }

    /// <inheritdoc />
    public override void LoadContent(ContentManager contentManager, GraphicsDevice? graphicsDevice = null)
    {
        base.LoadContent(contentManager, graphicsDevice);
        AnimationDefinition.FrameCount = (int)(Texture.Width / AnimationDefinition.FrameSize.Width);
    }

    /// <summary>
    /// Play the animation.
    /// </summary>
    /// <param name="loop">Whether or not to loop the animation.</param>
    /// <param name="cycleDirections">Whether or not the animation should also play right to left.</param>
    public void PlayAnimation(bool loop = false, bool cycleDirections = false)
    {
        Playing = true;
        AnimationDefinition.Loop = loop;
        AnimationDefinition.CycleDirections = cycleDirections;
    }

    /// <summary>
    /// Pause the animation on its current frame.
    /// </summary>
    public void PauseAnimation()
    {
        Playing = false;
    }

    /// <summary>
    /// Stop the animation, resetting the current frame to the beginning.
    /// </summary>
    public void StopAnimation()
    {
        Playing = false;
        _currentFrame = 0;
        _frameTimer = 0;
    }

    /// <inheritdoc />
    public override void Update(GameTime gameTime, KeyboardState? keyboardState = null)
    {
        if (Playing && _frameTimer >= AnimationDefinition.FrameDuration)
        {
            if (_animationDirection == AnimationDirection.LEFT_TO_RIGHT)
            {
                if (_currentFrame < (AnimationDefinition.FrameCount - 1))
                {
                    // Not the last frame, go to next frame
                    _currentFrame++;
                }
                else
                {
                    if (AnimationDefinition.CycleDirections)
                    {
                        // Last frame, start animating backwards
                        _animationDirection = AnimationDirection.RIGHT_TO_LEFT;
                        _currentFrame--;
                    }
                    else
                    {
                        // Last frame, reset to start and stop if not looping
                        _currentFrame = 0;
                        if (!AnimationDefinition.Loop)
                        {
                            StopAnimation();
                        }
                    }
                }
            }
            else if (_animationDirection == AnimationDirection.RIGHT_TO_LEFT)
            {
                if (_currentFrame > 0)
                {
                    // Not first frame, go to previous frame
                    _currentFrame--;
                }
                else
                {
                    if (AnimationDefinition.CycleDirections && AnimationDefinition.Loop)
                    {
                        // First frame, start animating forwards
                        _animationDirection = AnimationDirection.LEFT_TO_RIGHT;
                        _currentFrame++;
                    }
                    else
                    {
                        StopAnimation();
                    }
                }
            }

            _sourceRect.X = _currentFrame * (int)AnimationDefinition.FrameSize.Width;
            _sourceRect.Y = (int)AnimationRow * (int)AnimationDefinition.FrameSize.Height;
            _frameTimer = 0f;
        }
        else
        {
            _frameTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        base.Update(gameTime, keyboardState);
    }

    /// <inheritdoc />
    public override void Draw(SpriteBatch spriteBatch, Matrix? transformMatrix = null)
    {
        var texture = Texture;

        spriteBatch.Draw(
            texture,
            Position,
            _sourceRect,
            Color.White,
            0f,
            new Vector2(_sourceRect.Width / 2, _sourceRect.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    /// <inheritdoc />
    public override Rectangle GetBoundingRect()
    {
        var position = Position;
        return new Rectangle((int)position.X, (int)position.Y, _sourceRect.Width, _sourceRect.Height);
    }

    private int _currentFrame;
    private AnimationTextureRowIndex _currentRowIndex = AnimationTextureRowIndex.WALK_DOWN;
    private Rectangle _sourceRect;
    private AnimationDirection _animationDirection;

    private float _frameTimer;
}
