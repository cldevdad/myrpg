using System;
using System.Runtime.Serialization;

namespace MyRpg.Engine.Exceptions;

/// <summary>
/// The exception that is thrown when an attempt to access an entity fails due to the absence
/// of the entity.
/// </summary>
[Serializable]
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the EntityNotFoundException class.
    /// </summary>
    public EntityNotFoundException() { }

    /// <summary>
    /// Initializes a new instance of the EntityNotFoundException class with a specified error
    /// message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public EntityNotFoundException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the EntityNotFoundException class with a specified error
    /// message and a reference to the inner exception that is the cause of this exception..
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public EntityNotFoundException(string message, Exception inner)
        : base(message, inner) { }

    /// <summary>
    /// Initializes a new instance of the EntityNotFoundException class with serialized data.
    /// </summary>
    /// <param name="info">Serialized object data about the exception being thrown.</param>
    /// <param name="context">Contextual information about the source or destination.</param>
    protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
