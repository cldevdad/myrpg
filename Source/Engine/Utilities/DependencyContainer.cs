using System.Collections.Generic;
using MyRpg.Engine.Exceptions;

namespace MyRpg.Engine.Utilities;

/// <summary>
/// A container that provides a mechanism for registering and resolving dependencies
/// of different types.
/// </summary>
internal class DependencyContainer
{
    /// <summary>
    /// Register a dependency.
    /// </summary>
    /// <typeparam name="T">Dependency type.</typeparam>
    /// <param name="dependency">Dependency object.</param>
    public void Register<T>(T dependency)
        where T : notnull
    {
        RemoveDependency<T>();
        _dependencies.Add(dependency);
    }

    /// <summary>
    /// Un-register a dependency.
    /// </summary>
    /// <typeparam name="T">Dependency type.</typeparam>
    public void UnRegister<T>()
        where T : notnull
    {
        RemoveDependency<T>();
    }

    /// <summary>
    /// Resolve a dependency.
    /// </summary>
    /// <typeparam name="T">Dependency type.</typeparam>
    /// <returns>The dependency object.</returns>
    public T Resolve<T>()
        where T : notnull
    {
        T dependency =
            (T?)_dependencies.Find(d => d.GetType().IsAssignableTo(typeof(T)))
            ?? throw new DependencyNotFoundException(
                $"Could not resolve dependency: {typeof(T)}. Did you register it?"
            );
        return dependency;
    }

    /// <summary>
    /// Dependencies list.
    /// </summary>
    private readonly List<object> _dependencies = new List<object>();

    /// <summary>
    /// Remove a dependency from the dependencies list.
    /// </summary>
    /// <typeparam name="T">Dependency type.</typeparam>
    private void RemoveDependency<T>()
        where T : notnull
    {
        T? dependency = (T?)_dependencies.Find(d => d.GetType().IsAssignableTo(typeof(T)));
        if (dependency != null)
        {
            _dependencies.Remove(dependency);
        }
    }
}
