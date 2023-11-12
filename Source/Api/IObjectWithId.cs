namespace MyRpg.Api;

/// <summary>
/// Shape of an object with a unique identifier.
/// </summary>
public interface IObjectWithId
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    string Id { get; set; }
}
