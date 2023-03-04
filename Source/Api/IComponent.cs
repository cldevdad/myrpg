namespace MyRpg.Api;

/// <summary>
/// Shape of a constituent part of a game entity.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// The component's value.
    /// </summary>
    object Value { get; }

    /// <summary>
    /// Get the component's value.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <returns>The component's value.</returns>
    T GetValue<T>()
        where T : notnull;

    /// <summary>
    /// Set the component's value.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <param name="value">Value to set.</param>
    void SetValue<T>(T value)
        where T : notnull;
}
