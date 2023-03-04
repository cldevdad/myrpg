using System;
using System.Runtime.Serialization;

namespace MyRpg.Exceptions;

/// <summary>
/// The exception that is thrown when an attempt to access a map fails due to the absence
/// of the map.
/// </summary>
[Serializable]
public class MapNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the MapNotFoundException class.
    /// </summary>
    public MapNotFoundException() { }

    /// <summary>
    /// Initializes a new instance of the MapNotFoundException class with a specified error
    /// message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public MapNotFoundException(string? mapName)
        : base($"Map not found: {mapName}") { }

    /// <summary>
    /// Initializes a new instance of the MapNotFoundException class with a specified error
    /// message and a reference to the inner exception that is the cause of this exception..
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public MapNotFoundException(string? mapName, Exception inner)
        : base($"Map not found: {mapName}", inner) { }

    /// <summary>
    /// Initializes a new instance of the MapNotFoundException class with serialized data.
    /// </summary>
    /// <param name="info">Serialized object data about the exception being thrown.</param>
    /// <param name="context">Contextual information about the source or destination.</param>
    protected MapNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
