using System;
using System.Runtime.Serialization;

namespace MyRpg.Engine.Utilities;

public struct Size : IEquatable<Size>
{
    /// <summary>
    /// The width
    /// </summary>
    [DataMember]
    public float Width;

    [DataMember]
    public float Height;

    /// <summary>
    /// Initializes and retuns a new instance of the Size class with Width and Height set
    /// to the same value.
    /// </summary>
    /// <param name="value">Value to set the Width and Height to.</param>
    public Size(int value)
    {
        Width = Height = value;
    }

    /// <summary>
    /// Initializes and retuns a new instance of the Size class with Width and Height set
    /// to different values.
    /// </summary>
    /// <param name="width">Value to set the Width to.</param>
    /// <param name="height">Value to set the Height to.</param>
    public Size(float width, float height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Returns a Size with values 1, 1.
    /// </summary>
    public static Size One
    {
        get => new Size(1, 1);
    }

    /// <summary>
    /// Returns a Size with values 0,0.
    /// </summary>
    public static Size Zero
    {
        get => new Size(0, 0);
    }

    /// <summary>
    /// Compares whether the current size is equal to another size.
    /// </summary>
    /// <param name="other">Another Size instance.</param>
    /// <returns></returns>
    public bool Equals(Size other)
    {
        return other.Width == Width && other.Height == Height;
    }

    /// <summary>
    /// Multiplies the components of two sizes by each other.
    /// </summary>
    /// <param name="value1">The first size value.</param>
    /// <param name="value2">The second size value.</param>
    /// <returns>The result of the multiplication.</returns>
    public static Size operator *(Size value1, Size value2)
    {
        return new Size(value1.Width * value2.Width, value1.Height * value2.Height);
    }

    /// <summary>
    /// Multiplies the components of a size by a scalar.
    /// </summary>
    /// <param name="value">The size value.</param>
    /// <param name="scalar">The scalar value.</param>
    /// <returns>The result of the multiplication.</returns>
    public static Size operator *(Size value, float scalar)
    {
        return new Size(value.Width * scalar, value.Height * scalar);
    }

    /// <summary>
    /// Divides the components of a size by a scalar.
    /// </summary>
    /// <param name="value">The size value.</param>
    /// <param name="scalar">The scalar value.</param>
    /// <returns>The result of the multiplication.</returns>
    public static Size operator /(Size value, float scalar)
    {
        return new Size(value.Width / scalar, value.Height / scalar);
    }
}
