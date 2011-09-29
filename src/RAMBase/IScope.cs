
using System;

namespace RAMBase
{
  /// <summary>
  /// Represents a model scope. You should only perform actions against the model
  /// inside the scope and never after the scope has been disposed.
  /// </summary>
  /// <typeparam name="T">The type of the model.</typeparam>
  public interface IScope<T> : IDisposable
  {
    /// <summary>
    /// Gets wether this IScope has been disposed or not. Disposed scope will
    /// no longer guarantee thread safety.
    /// </summary>
    bool IsDisposed { get; }

    /// <summary>
    /// Gets the model instance.
    /// </summary>
    T Model { get; }
  }
}
