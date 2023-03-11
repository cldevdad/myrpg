using MyRpg.Engine.Utilities;

namespace MyRpg.Engine.Definitions;

/// <summary>
/// Definition of an AnimatedDisplayEntity.
/// </summary>
public class AnimatedDisplayEntityDefinition
{
    /// <summary>
    /// The width and height of a single animation frame.
    /// </summary>
    public Size FrameSize { get; set; }

    /// <summary>
    /// The amount each frame is displayed.
    /// </summary>
    public float FrameDuration { get; set; }

    /// <summary>
    /// The number of animation frames.
    /// </summary>
    public int FrameCount { get; set; }

    /// <summary>
    /// Whether or not the animation should loop while playing.
    /// </summary>
    public bool Loop { get; set; }

    /// <summary>
    /// Whether or not the animation should cycle left-to-right and right-to-left.
    /// </summary>
    public bool CycleDirections { get; set; }
}
