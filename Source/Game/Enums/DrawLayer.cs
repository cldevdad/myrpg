namespace MyRpg.Enums;

/// <summary>
/// Constants describing layer order for drawable entities.
/// </summary>
public enum DrawLayer
{
    /// <summary>
    /// Base layer.
    /// </summary>
    BASE,

    /// <summary>
    /// Map layer.
    /// </summary>
    MAP,

    /// <summary>
    /// First layer on top of the map.
    /// </summary>
    ACTOR_0,

    /// <summary>
    /// Second layer on top of the map.
    /// </summary>
    ACTOR_1,

    /// <summary>
    /// Third layer on top of the map.
    /// </summary>
    ACTOR_2,

    /// <summary>
    /// User interface layer.
    /// </summary>
    UI
}
