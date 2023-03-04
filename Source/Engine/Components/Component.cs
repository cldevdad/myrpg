using MyRpg.Api;

namespace MyRpg.Engine.Components;

/// <summary>
/// A constituent part of a game entity.
/// </summary>
public class Component : IComponent
{
    /// <inheritdoc />
    public string Id { get; }

    /// <inheritdoc />
    public object Value
    {
        get => _value;
        set
        {
            _valueType = value.GetType();
            _value = value;
        }
    }

    private object _value;

    private System.Type _valueType;

    /// <summary>
    /// Initializes a new component.
    /// </summary>
    /// <param name="id">The component's unique identifier.</param>
    /// <param name="valueType">The component's value type.</param>
    /// <param name="value">The component's value.</param>
    public Component(string id, System.Type valueType, object value)
    {
        Id = id;
        _valueType = valueType;
        _value = value;
    }

    /// <inheritdoc />
    public T GetValue<T>()
        where T : notnull
    {
        if (typeof(T) != _valueType)
        {
            System.Console.WriteLine($"Warning: Getting component value of type {_valueType} as type {typeof(T)}.");
        }

        return (T)Value;
    }

    /// <inheritdoc />
    public void SetValue<T>(T value)
        where T : notnull
    {
        _valueType = typeof(T);
        Value = value;
    }
}
