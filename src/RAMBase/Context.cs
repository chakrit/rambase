
using System.Collections.Generic;

namespace RAMBase
{
  internal partial class Context<T> : IContext<T>
  {
    private ModelContainer<T> _container;
    private Persistor<T> _persistor;

    private Stack<IScope<T>> _activeScopes;

    public Context(ModelContainer<T> container, Persistor<T> persistor)
    {
      _container = container;
      _persistor = persistor;
    }


    public IScope<T> CreateReadScope()
    { return filter(_container.CreateReadLockedScope()); }

    public IScope<T> CreateUpgradableReadScope()
    { return filter(_container.CreateUpgradableReadLockedScope()); }

    public IScope<T> CreateWriteScope()
    { return filter(_container.CreateWriteLockedScope()); }


    private IScope<T> filter(IScope<T> scope)
    {
      _activeScopes = _activeScopes ?? new Stack<IScope<T>>();
      _activeScopes.Push(scope);

      // TODO: CheckAndPersist should actually be called at disposal of scope
      //   instead of first access here
      _persistor.CheckAndPersist();
      return scope;
    }


    public void Dispose()
    {
      // purge all held scope when context is disposed
      while (_activeScopes.Count > 0) {
        var scope = _activeScopes.Pop();
        if (!scope.IsDisposed)
          scope.Dispose();
      }

      _activeScopes = null;
    }
  }
}
