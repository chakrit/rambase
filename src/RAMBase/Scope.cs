
using System;

namespace RAMBase
{
  internal class Scope<T> : IScope<T>, IDisposable
  {
    private T _model;

    private Action _onDispose;
    private bool _disposed;


    public bool IsDisposed { get { return _disposed; } }
    public T Model { get { return _model; } }

    public Scope(T model, Action onDispose)
    {
      _model = model;

      _onDispose = onDispose;
      _disposed = false;
    }


    void IDisposable.Dispose()
    {
      if (_disposed) return;
      _disposed = true;

      _model = default(T);
      _onDispose.Invoke();
    }
  }
}
